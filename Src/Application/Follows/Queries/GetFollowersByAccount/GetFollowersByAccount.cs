using Application.Accounts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Queries.GetFollowersByAccount;

public class GetFollowersByAccountQuery : IRequest<PaginatedList<AccountBriefDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFollowersByAccountQueryHandler : IRequestHandler<GetFollowersByAccountQuery, PaginatedList<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFollowersByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AccountBriefDto>> Handle(GetFollowersByAccountQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;

        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId,cancellationToken))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }
        
        return await _context.Follows
            .Where(f => f.FollowToId == accountId)
            .OrderByDescending(f => f.DateOfFollow)
            .Include(f => f.FollowFrom)
            .Select(f => f.FollowFrom)
            .ProjectTo<AccountBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
