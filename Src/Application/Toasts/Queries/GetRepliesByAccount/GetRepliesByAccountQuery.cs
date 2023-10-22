using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Queries.GetRepliesByAccount;

public class GetRepliesByAccountQuery : IRequest<PaginatedList<ToastBriefDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetRepliesByAccountQueryHandler : IRequestHandler<GetRepliesByAccountQuery, PaginatedList<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetRepliesByAccountQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<PaginatedList<ToastBriefDto>> Handle(GetRepliesByAccountQuery request, CancellationToken token)
    {
        var myAccountId = _userService.Id;
        var accountId = request.AccountId;
        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId, token))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }
        
        var accountReplies = await _context.Toasts
            .Where(t => t.AuthorId == accountId && t.Type != ToastType.Toast)
            .OrderByDescending(t => t.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Include(t => t.Replies)
            .Include(t => t.Reactions)
            .Include(t => t.Quotes)
            .Include(t => t.ReToasts)
            .ToArrayAsync(token);

        var selectedToasts = ToastSelectModel.SelectToasts(accountReplies, myAccountId)
            .Select(tsm => _mapper.Map<ToastBriefDto>(tsm));
        
        return selectedToasts.ToPaginatedArray(request.PageNumber, request.PageSize, totalCount);
    }
}