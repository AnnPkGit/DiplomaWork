using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts.Commands.CreateAccount;

public class BaseCreateAccountModelValidator : AbstractValidator<BaseCreateAccountModel>
{
    private const int MinimumLoginLenght = CommonAccountValidationRules.MinimumLoginLenght;
    private const int MaximumLoginLenght = CommonAccountValidationRules.MaximumLoginLenght;
    
    private const int MinimumNameLenght = CommonAccountValidationRules.MinimumNameLenght;
    private const int MaximumNameLenght = CommonAccountValidationRules.MaximumNameLenght;
    
    public BaseCreateAccountModelValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(MinimumLoginLenght)
            .MaximumLength(MaximumLoginLenght)
            .Matches(CommonAccountValidationRules.LoginRegex)
            .WithMessage(CommonAccountValidationRules.LoginRegexErrStr)
            .BeUniqueLogin(context).WithMessage(command => $"Login ({command.Login}) is already taken");
        
        RuleFor(v => v.Name)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(MinimumNameLenght)
            .MaximumLength(MaximumNameLenght)
            .When(command => !string.IsNullOrEmpty(command.Name));
    }
}