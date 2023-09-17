using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private const int MinimumPasswordLenght = CommonUserValidationRules.MinimumPasswordLenght;
    private const int MaximumPasswordLenght = CommonUserValidationRules.MaximumPasswordLenght;

    public CreateUserCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Matches(CommonUserValidationRules.EmailRegex)
            .BeUniqueEmail(context).WithMessage("This email is already taken");
        
        RuleFor(v => v.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(MinimumPasswordLenght)
            .MaximumLength(MaximumPasswordLenght)
            .Matches(CommonUserValidationRules.PasswordRegex)
            .WithMessage(CommonUserValidationRules.PassRegexErrStr);
    }
}