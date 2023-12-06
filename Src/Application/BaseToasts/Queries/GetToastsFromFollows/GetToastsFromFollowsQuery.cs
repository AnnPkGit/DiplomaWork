using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using Application.Quotes.Queries.Models;
using Application.Replies.Queries.Models;
using Application.ReToasts.Queries.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetToastsFromFollows;

[Authorize]
public class GetToastsFromFollowsQuery : IRequest<PaginatedList<BaseToastDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetToastsFromFollowsQueryHandler : IRequestHandler<GetToastsFromFollowsQuery, PaginatedList<BaseToastDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;

    public GetToastsFromFollowsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IMapper mapper)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BaseToastDto>> Handle(GetToastsFromFollowsQuery request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;

        var account = await _context.Accounts
            .FindAsync(new object?[] { accountId, }, cancellationToken);
        
        if (account == null)
        {
            throw new NotFoundException(nameof(Account), accountId);
        }
        
        var accountFollowIds = await _context.Accounts.Entry(account)
            .Collection(a => a.Follows)
            .Query()
            .Select(follow => follow.FollowToId)
            .ToArrayAsync(cancellationToken);

        if (!accountFollowIds.Any())
        {
            const int zeroCount = 0;
            return new PaginatedList<BaseToastDto>(Array.Empty<BaseToastDto>(), zeroCount, request.PageNumber, request.PageSize);
        }
        
        var baseFollowsToasts = await _context.BaseToasts
            .IgnoreAutoIncludes()
            .Where(toast => accountFollowIds.Contains(toast.AuthorId))
            .OrderByDescending(toast => toast.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .ToArrayAsync(cancellationToken);
        
        var toastIds = baseFollowsToasts
            .Where(toast => toast.Type == nameof(Toast))
            .Select(toast => toast.Id).ToArray();
        var replyIds = baseFollowsToasts
            .Where(toast => toast.Type == nameof(Reply))
            .Select(toast => toast.Id).ToArray();
        var quoteIds = baseFollowsToasts
            .Where(toast => toast.Type == nameof(Quote))
            .Select(toast => toast.Id).ToArray();
        var reToastIds = baseFollowsToasts
            .Where(toast => toast.Type == nameof(ReToast))
            .Select(toast => toast.Id).ToArray();
        
        var baseToastsDtos = new List<BaseToastDto>(baseFollowsToasts.Length);
        if (toastIds.Any())
        {
            var toasts = await _context.Toasts
                .IgnoreAutoIncludes()
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
            
            baseToastsDtos.AddRange(toasts);
        }
        if (replyIds.Any())
        {
            var replies = await _context.Replies
                .IgnoreAutoIncludes()
                .Where(reply => replyIds.Contains(reply.Id))
                .Include(reply => reply.Author).ThenInclude(author => author.Avatar)
                .Include(reply => reply.MediaItems)
                .Include(reply => reply.ReToasts)
                .Include(reply => reply.Reactions)
                .Include(reply => reply.Replies)
                .Include(reply => reply.Quotes)
                .Include(reply => reply.ReplyToToast)
                    .ThenInclude(toast => toast!.Author)
                    .ThenInclude(author => author.Avatar)
                .Include(reply => reply.ReplyToToast).ThenInclude(toast => toast!.ReToasts)
                .Include(reply => reply.ReplyToToast).ThenInclude(toast => toast!.Reactions)
                .Include(reply => reply.ReplyToToast).ThenInclude(toast => toast!.Replies)
                .Include(reply => reply.ReplyToToast).ThenInclude(toast => toast!.Quotes)
                .Include(reply => reply.ReplyToToast).ThenInclude(toast => toast!.MediaItems)
                .Select(reply => _mapper.Map<ReplyDto>(reply))
                .AsSingleQuery()
                .ToArrayAsync(cancellationToken);
            
            baseToastsDtos.AddRange(replies);
        }
        if (quoteIds.Any())
        {
            var quotes = await _context.Quotes
                .IgnoreAutoIncludes()
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
                .Include(quote => quote.QuotedToast).ThenInclude(toast => toast!.MediaItems)
                .Select(quote => _mapper.Map<QuoteDto>(quote))
                .AsSingleQuery()
                .ToArrayAsync(cancellationToken);
            
            baseToastsDtos.AddRange(quotes);
        }
        if (reToastIds.Any())
        {
            var reToasts = await _context.ReToasts
                .IgnoreAutoIncludes()
                .Where(reToast => reToastIds.Contains(reToast.Id))
                .Include(reToast => reToast.ToastWithContent)
                    .ThenInclude(toast => toast!.Author)
                    .ThenInclude(author => author.Avatar)
                .Include(reToast => reToast.ToastWithContent).ThenInclude(toast => toast!.MediaItems)
                .Include(reToast => reToast.ToastWithContent).ThenInclude(toast => toast!.ReToasts)
                .Include(reToast => reToast.ToastWithContent).ThenInclude(toast => toast!.Reactions)
                .Include(reToast => reToast.ToastWithContent).ThenInclude(toast => toast!.Replies)
                .Include(reToast => reToast.ToastWithContent).ThenInclude(toast => toast!.Quotes)
                .Select(reToast => _mapper.Map<ReToastDto>(reToast))
                .AsSingleQuery()
                .ToArrayAsync(cancellationToken);
            
            baseToastsDtos.AddRange(reToasts);
        }
        
        var items = baseToastsDtos
            .OrderByDescending(toast => toast.Created)
            .ToArray();
        
        return new PaginatedList<BaseToastDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}