using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reactions.Queries.GetReactionsByAccount;

public class GetReactionsByAccountQuery : IRequest<PaginatedList<ToastBriefDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetReactionsByAccountQueryHandler : IRequestHandler<GetReactionsByAccountQuery, PaginatedList<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetReactionsByAccountQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<PaginatedList<ToastBriefDto>> Handle(GetReactionsByAccountQuery request, CancellationToken cancellationToken)
    {
        var myAccountId = _userService.Id;
        var reactions = await _context.Reactions
            .Where(r => r.AuthorId == request.AccountId)
            .OrderByDescending(r => r.Reacted)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Include(r => r.Toast).ThenInclude(t => t!.Reactions)
            .Include(r => r.Toast).ThenInclude(t => t!.Replies)
            .Include(r => r.Toast).ThenInclude(t => t!.Quotes)
            .Include(r => r.Toast).ThenInclude(t => t!.ReToasts)
            .Select(r => r.Toast)
            .ToArrayAsync(cancellationToken);

        var selectedToast = ToastSelectModel.SelectToasts(reactions!, myAccountId)
            .Select(tsm => _mapper.Map<ToastBriefDto>(tsm));

        return selectedToast
            .ToPaginatedArray(request.PageNumber, request.PageSize, totalCount);
    }
}