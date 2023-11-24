using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Follows.Queries.Models;

namespace Application.Follows.Queries.GetFollowsCommands;


    public class GetAllFollowsCommand : IRequest<PaginatedList<FollowDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetAllFollowsQueryHandler : IRequestHandler<GetAllFollowsCommand, PaginatedList<FollowDto>>
    {
        private readonly ICurrentUserService _userService;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllFollowsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService userService)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<FollowDto>> Handle(GetAllFollowsCommand request, CancellationToken cancellationToken)
        {
                var userId = _userService.Id;
                var follows = _context.Follows
                .Where(f => f.AccountId == userId) // Получить всех, на кого подписан текущий пользователь
                .OrderByDescending(f => f.DateOfFollow)
                .ProjectTo<FollowDto>(_mapper.ConfigurationProvider);
                
          

            return await follows.PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
