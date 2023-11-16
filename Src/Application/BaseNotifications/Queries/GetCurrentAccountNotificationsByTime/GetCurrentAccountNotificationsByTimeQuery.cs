using Application.BaseNotifications.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseNotifications.Queries.GetCurrentAccountNotificationsByTime;

[Authorize]
public record GetCurrentAccountNotificationsByTimeQuery(DateTime Time) : IRequest<BaseNotificationDto[]>;

public class GetCurrentAccountNotificationsByTimeQueryHandler : IRequestHandler<GetCurrentAccountNotificationsByTimeQuery, BaseNotificationDto[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetCurrentAccountNotificationsByTimeQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }
    
    public async Task<BaseNotificationDto[]> Handle(GetCurrentAccountNotificationsByTimeQuery request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        
        var accountBaseNotifications = await _context.BaseNotifications
            .Where(bn => bn.ToAccountId == accountId && bn.Created.CompareTo(request.Time) > 0)
            .ToArrayAsync(cancellationToken);

        var result = BaseNotificationDto
            .ToBaseNotificationDto(accountBaseNotifications, _context, _mapper)
            .OrderByDescending(bn => bn.Created)
            .ToArray();

        return result;
    }
}