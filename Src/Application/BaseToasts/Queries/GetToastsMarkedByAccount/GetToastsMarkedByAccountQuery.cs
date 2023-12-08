using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Quotes.Queries.Models;
using Application.Replies.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetToastsMarkedByAccount;

public class GetToastsMarkedByAccountQuery : IRequest<PaginatedList<BaseToastWithContentBriefDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetToastsMarkedByAccountQueryHandler : IRequestHandler<GetToastsMarkedByAccountQuery, PaginatedList<BaseToastWithContentBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetToastsMarkedByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BaseToastWithContentBriefDto>> Handle(GetToastsMarkedByAccountQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts.AnyAsync(a => a.Id == request.AccountId, cancellationToken))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }
        
        var accountToastsWithContent = await _context.Reactions
            .Where(r => r.AuthorId == request.AccountId)
            .OrderByDescending(r => r.Reacted)
            .IgnoreAutoIncludes()
            .Include(r => r.ToastWithContent).ThenInclude(bt => bt.Author).ThenInclude(a => a!.Avatar)
            .Include(r => r.ToastWithContent).ThenInclude(bt => bt.Replies)
            .Include(r => r.ToastWithContent).ThenInclude(bt => bt.Reactions)
            .Include(r => r.ToastWithContent).ThenInclude(bt => bt.ReToasts)
            .Include(r => r.ToastWithContent).ThenInclude(bt => bt.Quotes)
            .Include(r => r.ToastWithContent).ThenInclude(bt => bt.MediaItems)
            .Select(r => r.ToastWithContent)
            .Where(bt => bt.Type != nameof(ReToast))
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .AsSingleQuery()
            .ToArrayAsync(cancellationToken);

        var toastWithContentDtos = new List<BaseToastWithContentBriefDto>(accountToastsWithContent.Length);
        foreach (var toastWithContent in accountToastsWithContent)
        {
            switch (toastWithContent)
            {
                case Reply entity:
                    await _context.Replies
                        .Entry(entity)
                        .Reference(reply => reply.ReplyToToast)
                        .LoadAsync(cancellationToken);
                    toastWithContentDtos.Add(_mapper.Map<ReplyBriefDto>(entity));
                    break;
                case Quote entity:
                    await _context.Quotes
                        .Entry(entity)
                        .Reference(quote => quote.QuotedToast)
                        .LoadAsync(cancellationToken);
                    toastWithContentDtos.Add(_mapper.Map<QuoteBriefDto>(entity));
                    break;
                default:
                    toastWithContentDtos.Add(_mapper.Map<BaseToastWithContentBriefDto>(toastWithContent));
                    break;
            }
        }
        
        return new PaginatedList<BaseToastWithContentBriefDto>(toastWithContentDtos, totalCount, request.PageNumber, request.PageSize);
    }
}