using Application.Accounts.Commands.CreateAccount;
using Application.Common.Interfaces;
using Application.Users.Commands.CreateUser;
using FluentValidation;

namespace Application.Users.Commands.RegistrateUserWithAccount;

public class RegistrateUserAndAccountCommandValidator : AbstractValidator<RegistrateUserAndAccountCommand>
{
    public RegistrateUserAndAccountCommandValidator(IApplicationDbContext context)
    {
        RuleFor(command => command.User).SetValidator(new BaseCreateUserModelValidator(context));
        RuleFor(command => command.Account).SetValidator(new BaseCreateAccountModelValidator(context));
    }
}