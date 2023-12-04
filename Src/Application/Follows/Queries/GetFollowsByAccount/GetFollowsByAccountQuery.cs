using Application.Accounts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Queries.GetFollowsByAccount;

public class GetFollowsByAccountQuery : IRequest<PaginatedList<AccountSearchDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFollowsByAccountQueryHandler : IRequestHandler<GetFollowsByAccountQuery, PaginatedList<AccountSearchDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetFollowsByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<AccountSearchDto>> Handle(GetFollowsByAccountQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;

        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId,cancellationToken))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }

        var items = await _context.Follows
            .IgnoreAutoIncludes()
            .Where(f => f.FollowFromId == accountId)
            .OrderByDescending(f => f.DateOfFollow)
            .Include(f => f.FollowTo).ThenInclude(a => a.Avatar)
            .Include(f => f.FollowTo).ThenInclude(a => a.Followers)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Select(f => _mapper.Map<AccountSearchDto>(f.FollowTo))
            .AsSingleQuery()
            .ToArrayAsync(cancellationToken);
        
        return new PaginatedList<AccountSearchDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}
