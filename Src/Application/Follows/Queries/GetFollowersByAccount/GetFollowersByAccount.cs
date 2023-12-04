using Application.Accounts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Queries.GetFollowersByAccount;

public class GetFollowersByAccountQuery : IRequest<PaginatedList<AccountSearchDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFollowersByAccountQueryHandler : IRequestHandler<GetFollowersByAccountQuery, PaginatedList<AccountSearchDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFollowersByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AccountSearchDto>> Handle(GetFollowersByAccountQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;

        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId,cancellationToken))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }

        var items = await _context.Follows
            .IgnoreAutoIncludes()
            .Where(f => f.FollowToId == accountId)
            .OrderByDescending(f => f.DateOfFollow)
            .Include(f => f.FollowFrom).ThenInclude(a => a.Avatar)
            .Include(f => f.FollowFrom).ThenInclude(a => a.Followers)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Select(f => _mapper.Map<AccountSearchDto>(f.FollowFrom))
            .AsSingleQuery()
            .ToArrayAsync(cancellationToken);
        
        return new PaginatedList<AccountSearchDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}
