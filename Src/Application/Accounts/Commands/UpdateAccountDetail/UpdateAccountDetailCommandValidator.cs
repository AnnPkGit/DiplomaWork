using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts.Commands.UpdateAccountDetail;

public class UpdateAccountDetailCommandValidator : AbstractValidator<UpdateAccountDetailCommand>
{
    private const int MinimumLoginLength = CommonAccountValidationRules.MinimumLoginLength;
    private const int MaximumLoginLength = CommonAccountValidationRules.MaximumLoginLength;
    
    private const int MaximumNameLength = CommonAccountValidationRules.MaximumNameLength;
    
    private const int MinimalMediaItemIdValue = -1;
    
    public UpdateAccountDetailCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Login)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(MinimumLoginLength)
            .MaximumLength(MaximumLoginLength)
            .Matches(CommonAccountValidationRules.LoginRegex)
            .WithMessage(CommonAccountValidationRules.LoginRegexErrStr)
            .BeUniqueLogin(context).WithMessage(command => $"Login ({command.Login}) is already taken")
            .When(command => command.Login != null);
        
        RuleFor(v => v.Name)
            .MaximumLength(MaximumNameLength)
            .When(command => !string.IsNullOrEmpty(command.Name));
        
        RuleFor(v => v.Bio)
            .MaximumLength(CommonAccountValidationRules.MaximumBioLength)
            .When(command => command.Bio != null);
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.AvatarId != null);
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.AvatarId != null);
    }
}