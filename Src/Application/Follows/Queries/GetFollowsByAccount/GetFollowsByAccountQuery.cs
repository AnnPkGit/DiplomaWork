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

namespace Application.Follows.Queries.GetFollowsByAccount;

public class GetFollowsByAccountQuery : IRequest<PaginatedList<AccountBriefDto>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFollowsByAccountQueryHandler : IRequestHandler<GetFollowsByAccountQuery, PaginatedList<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetFollowsByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<AccountBriefDto>> Handle(GetFollowsByAccountQuery request, CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;

        if (!await _context.Accounts.AnyAsync(a => a.Id == accountId,cancellationToken))
        {
            throw new NotFoundException(nameof(Account), accountId);
        }
        
        return await _context.Follows
            .Where(f => f.FollowFromId == accountId)
            .OrderByDescending(f => f.DateOfFollow)
            .Include(f => f.FollowTo)
            .Select(f => f.FollowTo)
            .ProjectTo<AccountBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
