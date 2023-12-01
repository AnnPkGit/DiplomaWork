using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.MuteNotificationOption;

namespace Application.Users.Commands.UpdateUserMuteNotificationOptions;

[Authorize]
public class UpdateUserMuteNotificationOptionsCommand : IRequest
{
    public bool? YouDoNotFollow { get; set; }
    public bool? WhoDoNotFollowYou { get; set; }
    public bool? WithANewAccount { get; set; }
    public bool? WhoHaveDefaultProfilePhoto { get; set; }
    public bool? WhoHaveNotConfirmedTheirEmail { get; set; }
    public bool? WhoHaveNotConfirmedTheirPhoneNumber { get; set; }
}

public class UpdateUserMuteNotificationOptionsCommandHandler : IRequestHandler<UpdateUserMuteNotificationOptionsCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;

    public UpdateUserMuteNotificationOptionsCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public async Task Handle(UpdateUserMuteNotificationOptionsCommand request, CancellationToken cancellationToken)
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

        var stateChanged = false;
        if (request.YouDoNotFollow != null)
        {
            var optionIsSet = user.MuteNotificationOptions.Any(option => option.Equals(YouDoNotFollow));
            if (request.YouDoNotFollow.Value)
            {
                if (!optionIsSet)
                {
                    user.MuteNotificationOptions.Add(YouDoNotFollow);
                    stateChanged = true;
                }
            }
            else
            {
                if (optionIsSet)
                {
                    user.MuteNotificationOptions.Remove(YouDoNotFollow);
                    stateChanged = true;
                }
            }
        }
        if (request.WhoDoNotFollowYou != null)
        {
            var optionIsSet = user.MuteNotificationOptions.Any(option => option.Equals(WhoDoNotFollowYou));
            if (request.WhoDoNotFollowYou.Value)
            {
                if (!optionIsSet)
                {
                    user.MuteNotificationOptions.Add(WhoDoNotFollowYou);
                    stateChanged = true;
                }
            }
            else
            {
                if (optionIsSet)
                {
                    user.MuteNotificationOptions.Remove(WhoDoNotFollowYou);
                    stateChanged = true;
                }
            }
        }
        if (request.WithANewAccount != null)
        {
            var optionIsSet = user.MuteNotificationOptions.Any(option => option.Equals(WithANewAccount));
            if (request.WithANewAccount.Value)
            {
                if (!optionIsSet)
                {
                    user.MuteNotificationOptions.Add(WithANewAccount);
                    stateChanged = true;
                }
            }
            else
            {
                if (optionIsSet)
                {
                    user.MuteNotificationOptions.Remove(WithANewAccount);
                    stateChanged = true;
                }
            }
        }
        if (request.WhoHaveDefaultProfilePhoto != null)
        {
            var optionIsSet = user.MuteNotificationOptions
                .Any(option => option.Equals(WhoHaveDefaultProfilePhoto));
            if (request.WhoHaveDefaultProfilePhoto.Value)
            {
                if (!optionIsSet)
                {
                    user.MuteNotificationOptions.Add(WhoHaveDefaultProfilePhoto);
                    stateChanged = true;
                }
            }
            else
            {
                if (optionIsSet)
                {
                    user.MuteNotificationOptions.Remove(WhoHaveDefaultProfilePhoto);
                    stateChanged = true;
                }
            }
        }
        if (request.WhoHaveNotConfirmedTheirEmail != null)
        {
            var optionIsSet = user.MuteNotificationOptions
                .Any(option => option.Equals(WhoHaveNotConfirmedTheirEmail));
            if (request.WhoHaveNotConfirmedTheirEmail.Value)
            {
                if (!optionIsSet)
                {
                    user.MuteNotificationOptions.Add(WhoHaveNotConfirmedTheirEmail);
                    stateChanged = true;
                }
            }
            else
            {
                if (optionIsSet)
                {
                    user.MuteNotificationOptions.Remove(WhoHaveNotConfirmedTheirEmail);
                    stateChanged = true;
                }
            }
        }
        if (request.WhoHaveNotConfirmedTheirPhoneNumber != null)
        {
            var optionIsSet = user.MuteNotificationOptions
                .Any(option => option.Equals(WhoHaveNotConfirmedTheirPhoneNumber));
            if (request.WhoHaveNotConfirmedTheirPhoneNumber.Value)
            {
                if (!optionIsSet)
                {
                    user.MuteNotificationOptions.Add(WhoHaveNotConfirmedTheirPhoneNumber);
                    stateChanged = true;
                }
            }
            else
            {
                if (optionIsSet)
                {
                    user.MuteNotificationOptions.Remove(WhoHaveNotConfirmedTheirPhoneNumber);
                    stateChanged = true;
                }
            }
        }

        if (stateChanged)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}