
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Follows.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Follows.Queries.GetFollowsCommands;

public class GetFollowersCommand : IRequest<PaginatedList<FollowDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFollowersQueryHandler : IRequestHandler<GetFollowersCommand, PaginatedList<FollowDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetFollowersQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<FollowDto>> Handle(GetFollowersCommand request, CancellationToken cancellationToken)
    {
        int currentUserId = _currentUserService.Id;

        var followers = _context.Follows
            .Where(f => f.ToAccountId == currentUserId)
            .OrderByDescending(f => f.DateOfFollow)
            .ProjectTo<FollowDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return await followers;
    }
}
