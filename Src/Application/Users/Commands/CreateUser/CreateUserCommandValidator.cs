using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<BaseCreateUserModel>
{
    public CreateUserCommandValidator(IApplicationDbContext context)
    {
        Include(new BaseCreateUserModelValidator(context));
    }
}