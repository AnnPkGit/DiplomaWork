using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    private const int MinimumLoginLength = CommonAccountValidationRules.MinimumLoginLength;
    private const int MaximumLoginLength = CommonAccountValidationRules.MaximumLoginLength;
    
    private const int MinimumNameLength = CommonAccountValidationRules.MinimumNameLength;
    private const int MaximumNameLength = CommonAccountValidationRules.MaximumNameLength;

    private const int MinimalMediaItemIdValue = -1;
    public UpdateAccountCommandValidator(IApplicationDbContext context)
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
            .MinimumLength(MinimumNameLength)
            .MaximumLength(MaximumNameLength)
            .When(command => !string.IsNullOrEmpty(command.Name));

        RuleFor(v => v.Bio)
            .MaximumLength(CommonAccountValidationRules.MaximumBioLength)
            .When(command => !string.IsNullOrEmpty(command.Bio));
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.AvatarId != null);
        
        RuleFor(command => command.AvatarId)
            .GreaterThan(MinimalMediaItemIdValue)
            .When(command => command.BannerId != null);
    }
}