using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts.Commands.UpdateAccountDetail;

public class UpdateAccountDetailCommandValidator : AbstractValidator<UpdateAccountDetailCommand>
{
    private const int MinimumLoginLenght = CommonAccountValidationRules.MinimumLoginLenght;
    private const int MaximumLoginLenght = CommonAccountValidationRules.MaximumLoginLenght;
    
    private const int MaximumNameLenght = CommonAccountValidationRules.MaximumNameLenght;
    
    private const int MinimalMediaItemIdValue = -1;
    
    public UpdateAccountDetailCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Login)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(MinimumLoginLenght)
            .MaximumLength(MaximumLoginLenght)
            .Matches(CommonAccountValidationRules.LoginRegex)
            .WithMessage(CommonAccountValidationRules.LoginRegexErrStr)
            .BeUniqueLogin(context).WithMessage(command => $"Login ({command.Login}) is already taken")
            .When(command => command.Login != null);
        
        RuleFor(v => v.Name)
            .MaximumLength(MaximumNameLenght)
            .When(command => !string.IsNullOrEmpty(command.Name));
        
        RuleFor(v => v.Bio)
            .MaximumLength(CommonAccountValidationRules.MaximumBioLenght)
            .When(command => command.Bio != null);
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.AvatarId != null);
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.AvatarId != null);
    }
}