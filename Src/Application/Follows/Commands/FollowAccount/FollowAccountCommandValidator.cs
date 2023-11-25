using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Follows.Commands.FollowAccount;

public class FollowAccountCommandValidator : AbstractValidator<FollowAccountCommand>
{
    public FollowAccountCommandValidator(ICurrentUserService userService)
    {
        var currentAccountId = userService.Id;

        RuleFor(command => command.AccountId)
            .NotEmpty()
            .NotEqual(currentAccountId);
    }
}