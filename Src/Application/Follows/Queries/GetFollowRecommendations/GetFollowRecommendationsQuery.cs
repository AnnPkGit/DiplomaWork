using Application.Accounts.Queries.Models;
using Application.Common.Constants;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Queries.GetFollowRecommendations;

public class GetFollowRecommendationsQuery : IRequest<IEnumerable<AccountSearchDto>>
{
    public int PageSize { get; init; } = 5;
}

public class GetFollowRecommendationsQueryHandler : IRequestHandler<GetFollowRecommendationsQuery, IEnumerable<AccountSearchDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetFollowRecommendationsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<IEnumerable<AccountSearchDto>> Handle(GetFollowRecommendationsQuery request, CancellationToken cancellationToken)
    {
        int accountsCount = request.PageSize;
        int accountId = _userService.Id;

        int[] accountFollowToIds;
        if (accountId != UserDefaultValues.Id)
        {
            accountFollowToIds = await _context.Follows
                .Where(follow => follow.FollowFromId == accountId)
                .Select(follow => follow.FollowToId)
                .ToArrayAsync(cancellationToken);
        }
        else
        {
            accountFollowToIds = Array.Empty<int>();
        }
        
        AccountSearchDto[] resultDtos = await _context.Accounts
            .IgnoreAutoIncludes()
            .Where(account => !accountFollowToIds.Contains(account.Id) && account.Id != accountId)
            .OrderBy(account => EF.Functions.Random())
            .Take(accountsCount)
            .Include(account => account.Avatar)
            .Include(account => account.Followers)
            .Select(account => _mapper.Map<AccountSearchDto>(account))
            .AsSingleQuery()
            .ToArrayAsync(cancellationToken);
        
        return resultDtos;
    }
}