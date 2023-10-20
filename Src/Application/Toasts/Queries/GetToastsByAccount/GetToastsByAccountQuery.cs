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

namespace Application.Toasts.Queries.GetToastsByAccount;

public class GetToastsByAccountQuery : IRequest<PaginatedList<ToastBriefDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAccountToastsByIdQueryHandler : IRequestHandler<GetToastsByAccountQuery, PaginatedList<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetAccountToastsByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<PaginatedList<ToastBriefDto>> Handle(GetToastsByAccountQuery request, CancellationToken token)
    {
        var myAccountId = _userService.Id;
        var accountId = request.AccountId;
        
        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId, token))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }
        
        var toasts = await _context.Toasts
            .Where(t => t.AuthorId == accountId && t.Type != ToastType.Reply)
            .OrderByDescending(t => t.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Include(t => t.Replies).ThenInclude(t => t.Replies)
            .Include(t => t.Replies).ThenInclude(t => t.Reactions)
            .Include(t => t.Replies).ThenInclude(t => t.Quotes)
            .Include(t => t.Replies).ThenInclude(t => t.ReToasts)
            .Include(t => t.Reactions)
            .Include(t => t.Quotes)
            .Include(t => t.ReToasts)
            .ToArrayAsync(token);

        var selectedToasts = ToastSelectModel.SelectToasts(toasts, myAccountId)
            .Select(tsm => _mapper.Map<ToastBriefDto>(tsm));
        
        return selectedToasts.ToPaginatedArray(request.PageNumber, request.PageSize, totalCount);
    }

}