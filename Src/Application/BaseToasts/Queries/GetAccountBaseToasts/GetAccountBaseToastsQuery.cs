using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Quotes.Queries.Models;
using Application.ReToasts.Queries.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetAccountBaseToasts;

public class GetAccountBaseToastsQuery : IRequest<PaginatedList<BaseToastDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAccountBaseToastsQueryHandler : IRequestHandler<GetAccountBaseToastsQuery, PaginatedList<BaseToastDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountBaseToastsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BaseToastDto>> Handle(GetAccountBaseToastsQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;
        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId, cancellationToken))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }

        var accountBaseToasts = await _context.BaseToasts
            .Where(bt => bt.AuthorId == request.AccountId && bt.Type != nameof(Reply))
            .OrderByDescending(bt => bt.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .ToArrayAsync(cancellationToken);

        var accountToastIds = accountBaseToasts
            .Where(bt => bt.Type == nameof(Toast))
            .Select(bt => bt.Id).ToArray();
        var accountQuoteIds = accountBaseToasts
            .Where(bt => bt.Type == nameof(Quote))
            .Select(bt => bt.Id).ToArray();
        var accountReToastIds = accountBaseToasts
            .Where(bt => bt.Type == nameof(ReToast))
            .Select(bt => bt.Id).ToArray();
        
        var objectsDto = new List<BaseToastDto>(accountBaseToasts.Length);
        if (accountToastIds.Any())
        {
            var toasts = _context.Toasts
                .IgnoreAutoIncludes()
                .Where(t => accountToastIds.Contains(t.Id))
                .Include(t => t.Author).ThenInclude(a => a!.Avatar)
                .Include(t => t.MediaItems)
                .Include(t => t.Replies)
                .Include(t => t.Reactions)
                .Include(t => t.ReToasts)
                .Include(t => t.Quotes)
                .AsSingleQuery();
            var toastsDto = toasts.Select(t => _mapper.Map<ToastDto>(t));
            objectsDto.AddRange(toastsDto);
        }
        if (accountQuoteIds.Any())
        {
            var quotes = _context.Quotes
                .IgnoreAutoIncludes()
                .Where(q => accountQuoteIds.Contains(q.Id))
                .Include(q => q.Author).ThenInclude(a => a!.Avatar)
                .Include(q => q.MediaItems)
                .Include(q => q.Replies)
                .Include(q => q.Reactions)
                .Include(q => q.ReToasts)
                .Include(q => q.Quotes)
                .Include(q => q.QuotedToast)
                    .ThenInclude(t => t!.Author)
                    .ThenInclude(a => a!.Avatar)
                .Include(q => q.QuotedToast).ThenInclude(t => t!.MediaItems)
                .AsSingleQuery();
            var quotesDto = quotes.Select(t => _mapper.Map<QuoteDto>(t));
            objectsDto.AddRange(quotesDto);
        }
        if (accountReToastIds.Any())
        {
            var reToasts = _context.ReToasts
                .IgnoreAutoIncludes()
                .Where(rt => accountReToastIds.Contains(rt.Id))
                .Include(rt => rt.Author).ThenInclude(a => a!.Avatar)
                .Include(rt => rt.ToastWithContent)
                    .ThenInclude(t => t!.Author)
                    .ThenInclude(a => a!.Avatar)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t!.MediaItems)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t!.Replies)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t!.Reactions)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t!.ReToasts)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t!.Quotes)
                .AsSingleQuery();
            var reToastsDto = reToasts.Select(t => _mapper.Map<ReToastDto>(t));
            objectsDto.AddRange(reToastsDto);
        }
        
        return new PaginatedList<BaseToastDto>(objectsDto.OrderByDescending(bt => bt.Created).ToArray(), totalCount, request.PageNumber, request.PageSize);
    }
}