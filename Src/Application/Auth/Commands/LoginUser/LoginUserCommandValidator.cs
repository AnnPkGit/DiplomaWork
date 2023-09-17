using Application.Users;
using FluentValidation;

namespace Application.Auth.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    private const int MinimumPasswordLenght = CommonUserValidationRules.MinimumPasswordLenght;
    private const int MaximumPasswordLenght = CommonUserValidationRules.MaximumPasswordLenght;
    
    public LoginUserCommandValidator()
    {
        var lenghtPassErrStr = $"The Password must be at least {MinimumPasswordLenght} and at max {MaximumPasswordLenght} characters long.";
        RuleFor(v => v.Email)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
        RuleFor(v => v.Password)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .MinimumLength(MinimumPasswordLenght)
            .MaximumLength(MaximumPasswordLenght)
            .Matches(@"^(?=.*\p{Lu})(?=.*\p{Ll})(?=.*\d)(?=.*[^\p{L}\p{N}]).{0,}$");
    }
}