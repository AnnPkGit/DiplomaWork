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
    private readonly ICurrentUserService _currentUserService;
    
    public CreateAccountCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
        
        RuleFor(v => v.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(MinimumLoginLenght)
            .MaximumLength(MaximumLoginLenght)
            .Matches(CommonAccountValidationRules.LoginRegex)
            .WithMessage(CommonAccountValidationRules.LoginRegexErrStr)
            .MustAsync(HaveAccountsAvailable).WithMessage("Accounts limit exceeded")
            .BeUniqueLogin(_context).WithMessage(command => $"Login ({command.Login}) is already taken");
        
        RuleFor(v => v.Name)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(MinimumNameLenght)
            .MaximumLength(MaximumNameLenght)
            .When(command => !string.IsNullOrEmpty(command.Name));
    }
    private async Task<bool> HaveAccountsAvailable(string login, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var currentUser = await _context.Users
            .Include(u => u.Accounts)
            .SingleOrDefaultAsync(u => u.Id == userId, token);
        
        if (currentUser == null)
            throw new NotFoundException(nameof(User), userId);
        return currentUser.MaxAccountsCount > currentUser.Accounts.Count;
    }
}