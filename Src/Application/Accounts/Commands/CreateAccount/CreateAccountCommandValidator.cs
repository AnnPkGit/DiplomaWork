using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    private const int MinimumLoginLenght = CommonAccountValidationRules.MinimumLoginLenght;
    private const int MaximumLoginLenght = CommonAccountValidationRules.MaximumLoginLenght;
    
    private const int MinimumNameLenght = CommonAccountValidationRules.MinimumNameLenght;
    private const int MaximumNameLenght = CommonAccountValidationRules.MaximumNameLenght;
    
    private readonly IApplicationDbContext _context;
    
    public CreateAccountCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(HaveNotAccount).WithMessage("You already have the account");
        
        RuleFor(v => v.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(MinimumLoginLenght)
            .MaximumLength(MaximumLoginLenght)
            .Matches(CommonAccountValidationRules.LoginRegex)
            .WithMessage(CommonAccountValidationRules.LoginRegexErrStr)
            .BeUniqueLogin(_context).WithMessage(command => $"Login ({command.Login}) is already taken");
        
        RuleFor(v => v.Name)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(MinimumNameLenght)
            .MaximumLength(MaximumNameLenght)
            .When(command => !string.IsNullOrEmpty(command.Name));
    }
    private async Task<bool> HaveNotAccount(int userId, CancellationToken token)
    {
        return !await _context.Accounts.AnyAsync(account => account.Id == userId, token);
    }
}