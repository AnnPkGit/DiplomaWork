using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Quotes.Queries.Models;
using Application.Replies.Queries.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetToastsWithMediaItemsByAccount;

public class GetToastsWithMediaItemsByAccountQuery : IRequest<PaginatedList<BaseToastWithContentDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetToastsWithMediaItemsByAccountQueryHandler : IRequestHandler<GetToastsWithMediaItemsByAccountQuery, PaginatedList<BaseToastWithContentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetToastsWithMediaItemsByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BaseToastWithContentDto>> Handle(GetToastsWithMediaItemsByAccountQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;
        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId, cancellationToken))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }

        var toastWithMediaItemIds = await _context.BaseToastsWithContent
            .IgnoreAutoIncludes()
            .Include(toast => toast.MediaItems)
            .Where(toast => toast.AuthorId == accountId && toast.MediaItems.Count > 0)
            .OrderByDescending(toast => toast.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .ToArrayAsync(cancellationToken);

        var toastIds = toastWithMediaItemIds
            .Where(toast => toast.Type == nameof(Toast))
            .Select(toast => toast.Id).ToArray();
        var replyIds = toastWithMediaItemIds
            .Where(toast => toast.Type == nameof(Reply))
            .Select(toast => toast.Id).ToArray();
        var quoteIds = toastWithMediaItemIds
            .Where(toast => toast.Type == nameof(Quote))
            .Select(toast => toast.Id).ToArray();

        var toastWithMediaItemDtos = new List<BaseToastWithContentDto>(toastWithMediaItemIds.Length);
        if (toastIds.Any())
        {
            var toasts = await _context.Toasts
                .Where(toast => toastIds.Contains(toast.Id))
                .Include(toast => toast.Author).ThenInclude(author => author.Avatar)
                .Include(toast => toast.MediaItems)
                .Include(toast => toast.ReToasts)
                .Include(toast => toast.Reactions)
                .Include(toast => toast.Replies)
                .Include(toast => toast.Quotes)
                .Select(toast => _mapper.Map<ToastDto>(toast))
                .AsSingleQuery()
                .ToArrayAsync(cancellationToken);
            
            toastWithMediaItemDtos.AddRange(toasts);
        }
        if (replyIds.Any())
        {
            var replies = await _context.Replies
                .Where(reply => replyIds.Contains(reply.Id))
                .Include(reply => reply.Author).ThenInclude(author => author.Avatar)
                .Include(reply => reply.MediaItems)
                .Include(reply => reply.ReToasts)
                .Include(reply => reply.Reactions)
                .Include(reply => reply.Replies)
                .Include(reply => reply.Quotes)
                .Include(reply => reply.ReplyToToast).ThenInclude(toast => toast!.Author)
                .Select(reply => _mapper.Map<ReplyDto>(reply))
                .AsSingleQuery()
                .ToArrayAsync(cancellationToken);
            
            toastWithMediaItemDtos.AddRange(replies);
        }
        if (quoteIds.Any())
        {
            var quotes = await _context.Quotes
                .Where(quote => quoteIds.Contains(quote.Id))
                .Include(quote => quote.Author).ThenInclude(author => author.Avatar)
                .Include(quote => quote.MediaItems)
                .Include(quote => quote.ReToasts)
                .Include(quote => quote.Reactions)
                .Include(quote => quote.Replies)
                .Include(quote => quote.Quotes)
                .Include(quote => quote.QuotedToast)
                    .ThenInclude(toast => toast!.Author)
                    .ThenInclude(author => author.Avatar)
                .Include(quote => quote.QuotedToast)
                    .ThenInclude(toast => toast!.MediaItems)
                .Select(quote => _mapper.Map<QuoteDto>(quote))
                .AsSingleQuery()
                .ToArrayAsync(cancellationToken);
            
            toastWithMediaItemDtos.AddRange(quotes);
        }

        var items = toastWithMediaItemDtos
            .OrderByDescending(toast => toast.Created)
            .ToArray();
        
        return new PaginatedList<BaseToastWithContentDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}