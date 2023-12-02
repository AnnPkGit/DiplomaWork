using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Users.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetCurrentUserMuteNotificationOptions;

[Authorize]
public record GetCurrentUserMuteNotificationOptionsQuery : IRequest<IEnumerable<MuteNotificationOptionDto>>;

public class GetCurrentUserMuteNotificationOptionsQueryHandler : IRequestHandler<GetCurrentUserMuteNotificationOptionsQuery, IEnumerable<MuteNotificationOptionDto>>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrentUserMuteNotificationOptionsQueryHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IMapper mapper)
    {
        _userService = userService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MuteNotificationOptionDto>> Handle(GetCurrentUserMuteNotificationOptionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var user = await _context.Users
            .Include(u => u.MuteNotificationOptions)
            .AsSingleQuery()
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }
        
        var allOptions = MuteNotificationOption.GetValues();
        var allOptionDtos = allOptions
            .Select(option => _mapper.Map<MuteNotificationOptionDto>(option))
            .ToArray();
        var userOptions = user.MuteNotificationOptions;
        foreach (var userOption in userOptions)
        {
            foreach (var optionDto in allOptionDtos)
            {
                if (optionDto.Id == userOption.Id)
                {
                    optionDto.Active = true;
                }
            }
        }

        return allOptionDtos;
    }
}