using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetRepliesByAccount;

public class GetRepliesByAccountQuery : IRequest<PaginatedList<object>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetRepliesByAccountQueryHandler : IRequestHandler<GetRepliesByAccountQuery, PaginatedList<object>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepliesByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<object>> Handle(GetRepliesByAccountQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts.AnyAsync(a => a.Id == request.AccountId, cancellationToken))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }
        
        var accountBaseToasts = await _context.BaseToasts
            .Where(bt => bt.AuthorId == request.AccountId && bt.Type != nameof(Toast))
            .OrderByDescending(bt => bt.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .ToArrayAsync(cancellationToken);
        
        var accountQuoteIds = accountBaseToasts
            .Where(bt => bt.Type == nameof(Quote))
            .Select(bt => bt.Id).ToArray();
        var accountReToastIds = accountBaseToasts
            .Where(bt => bt.Type == nameof(ReToast))
            .Select(bt => bt.Id).ToArray();
        var accountRepliesIds = accountBaseToasts
            .Where(bt => bt.Type == nameof(Reply))
            .Select(bt => bt.Id).ToArray();

        var objects = new List<BaseToast>(accountBaseToasts.Length);
        if (accountQuoteIds.Any())
        {
            var toasts = _context.Quotes
                .Where(q => accountQuoteIds.Contains(q.Id))
                .IgnoreAutoIncludes()
                .Include(t => t.Replies)
                .Include(t => t.Reactions)
                .Include(t => t.ReToasts)
                .Include(t => t.Quotes);
            objects.AddRange(toasts);
        }
        if (accountReToastIds.Any())
        {
            var toasts = _context.ReToasts
                .Where(rt => accountReToastIds.Contains(rt.Id))
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t.Replies)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t.Reactions)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t.ReToasts)
                .Include(rt => rt.ToastWithContent).ThenInclude(t => t.Quotes);
            objects.AddRange(toasts);
        }
        if (accountRepliesIds.Any())
        {
            var toasts = _context.Replies
                .Where(r => accountRepliesIds.Contains(r.Id))
                .Include(r => r.ReplyToToast).ThenInclude(t => t.Replies)
                .Include(r => r.ReplyToToast).ThenInclude(t => t.Reactions)
                .Include(r => r.ReplyToToast).ThenInclude(t => t.ReToasts)
                .Include(r => r.ReplyToToast).ThenInclude(t => t.Quotes);
            objects.AddRange(toasts);
        }
        
        var destinationType = typeof(BaseToastDto);
        
        var selectedBaseToasts = objects
            .OrderByDescending(bt => bt.Created)
            .Select(bt => _mapper.Map(bt, bt.GetType(), destinationType))
            .ToArray();

        return new PaginatedList<object>(selectedBaseToasts, totalCount, request.PageNumber, request.PageSize);
    }
}

