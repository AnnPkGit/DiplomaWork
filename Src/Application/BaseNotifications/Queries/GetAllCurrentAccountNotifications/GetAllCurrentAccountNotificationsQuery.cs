using Application.Common.Interfaces;
using Application.Common.Security;
using Application.BaseNotifications.Queries.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseNotifications.Queries.GetAllCurrentAccountNotifications;

[Authorize]
public record GetAllCurrentAccountNotificationsQuery : IRequest<BaseNotificationDto[]>;

public class GetAllCurrentAccountNotificationsQueryHandler : IRequestHandler<GetAllCurrentAccountNotificationsQuery, BaseNotificationDto[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetAllCurrentAccountNotificationsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<BaseNotificationDto[]> Handle(GetAllCurrentAccountNotificationsQuery request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        
        var accountBaseNotifications = await _context.BaseNotifications
            .Where(bn => bn.ToAccountId == accountId)
            .ToArrayAsync(cancellationToken);

        var result = BaseNotificationDto
            .ToBaseNotificationDto(accountBaseNotifications, _context, _mapper)
            .OrderByDescending(bn => bn.Created)
            .ToArray();
        
        return result;
    }
}