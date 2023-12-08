using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts.Commands.CreateAccount;

public class BaseCreateAccountModelValidator : AbstractValidator<BaseCreateAccountModel>
{
    private const int MinimumLoginLength = CommonAccountValidationRules.MinimumLoginLength;
    private const int MaximumLoginLength = CommonAccountValidationRules.MaximumLoginLength;
    
    private const int MinimumNameLength = CommonAccountValidationRules.MinimumNameLength;
    private const int MaximumNameLength = CommonAccountValidationRules.MaximumNameLength;
    
    public BaseCreateAccountModelValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(MinimumLoginLength)
            .MaximumLength(MaximumLoginLength)
            .Matches(CommonAccountValidationRules.LoginRegex)
            .WithMessage(CommonAccountValidationRules.LoginRegexErrStr)
            .BeUniqueLogin(context).WithMessage(command => $"Login ({command.Login}) is already taken");
        
        RuleFor(v => v.Name)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(MinimumNameLength)
            .MaximumLength(MaximumNameLength)
            .When(command => !string.IsNullOrEmpty(command.Name));
    }
}