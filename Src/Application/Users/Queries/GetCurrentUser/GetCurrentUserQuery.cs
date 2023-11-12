using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Users.Queries.GetCurrentUser;

[Authorize]
public record GetCurrentUserQuery : IRequest<UserBriefDto>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<UserBriefDto> Handle(GetCurrentUserQuery request, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var user = await _context.Users.FindAsync(new object?[] { userId, token }, token);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        return _mapper.Map<UserBriefDto>(user);
    }
}