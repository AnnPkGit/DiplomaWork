using Application.BaseNotifications.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseNotifications.Queries.GetCurrentAccountNotifications;

[Authorize]
public class GetCurrentAccountNotificationsQuery : IRequest<PaginatedList<BaseNotificationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAccountNotificationQueryHandler : IRequestHandler<GetCurrentAccountNotificationsQuery, PaginatedList<BaseNotificationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetAccountNotificationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<PaginatedList<BaseNotificationDto>> Handle(GetCurrentAccountNotificationsQuery request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        
        var accountBaseNotifications = await _context.BaseNotifications
            .Where(bn => bn.ToAccountId == accountId)
            .OrderByDescending(bn => bn.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .ToArrayAsync(cancellationToken);
        
        var objectsDto = BaseNotificationDto
            .ToBaseNotificationDto(accountBaseNotifications, _context, _mapper)
            .OrderByDescending(bn => bn.Created)
            .ToArray();
        
        return new PaginatedList<BaseNotificationDto>(objectsDto, totalCount, request.PageNumber, request.PageSize);
    }
}