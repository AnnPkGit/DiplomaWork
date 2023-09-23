using Application.Users;
using FluentValidation;

namespace Application.Auth.Commands.LoginUserUsingSession;

public class LoginUserUsingSessionCommandValidator : AbstractValidator<LoginUserUsingSessionCommand>
{
    private const int MinimumPasswordLenght = CommonUserValidationRules.MinimumPasswordLenght;
    private const int MaximumPasswordLenght = CommonUserValidationRules.MaximumPasswordLenght;
    
    public LoginUserUsingSessionCommandValidator()
    {
        var lenghtPassErrStr = $"The Password must be at least {MinimumPasswordLenght} and at max {MaximumPasswordLenght} characters long.";
        RuleFor(v => v.Email)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .Matches(CommonUserValidationRules.EmailRegex);
        RuleFor(v => v.Password)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .MinimumLength(MinimumPasswordLenght)
            .MaximumLength(MaximumPasswordLenght)
            .Matches(CommonUserValidationRules.PasswordRegex);
    }
}