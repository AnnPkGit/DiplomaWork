using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    private const int MinimumLoginLenght = CommonAccountValidationRules.MinimumLoginLenght;
    private const int MaximumLoginLenght = CommonAccountValidationRules.MaximumLoginLenght;
    
    private const int MinimumNameLenght = CommonAccountValidationRules.MinimumNameLenght;
    private const int MaximumNameLenght = CommonAccountValidationRules.MaximumNameLenght;

    private const int MinimalMediaItemIdValue = -1;
    public UpdateAccountCommandValidator(IApplicationDbContext context)
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
            .MinimumLength(MinimumNameLenght)
            .MaximumLength(MaximumNameLenght)
            .When(command => !string.IsNullOrEmpty(command.Name));
        
        // TODO: Implement validation for the Avatar field

        RuleFor(v => v.Bio)
            .MaximumLength(CommonAccountValidationRules.MaximumBioLenght)
            .When(command => !string.IsNullOrEmpty(command.Bio));
        
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.AvatarId != null);
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.BannerId != null);
    }
}