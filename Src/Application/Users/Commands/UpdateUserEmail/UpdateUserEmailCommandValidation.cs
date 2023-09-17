using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Users.Commands.UpdateUserEmail;

public class UpdateUserEmailCommandValidation : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidation(IApplicationDbContext context)
    {
        RuleFor(v => v.NewEmail)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Matches(CommonUserValidationRules.EmailRegex)
            .BeUniqueEmail(context).WithMessage("This email is already taken");
    }
}