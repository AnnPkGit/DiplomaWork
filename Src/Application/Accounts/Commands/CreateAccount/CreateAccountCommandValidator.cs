using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    
    private readonly IApplicationDbContext _context;
    
    public CreateAccountCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(HaveNotAccount).WithMessage("You already have an account or this user does not exist");
        
        Include(new BaseCreateAccountModelValidator(context));
    }
    private async Task<bool> HaveNotAccount(int userId, CancellationToken token)
    {
        return await _context.Users.AnyAsync(user => user.Id == userId && user.Account == null, token);
    }
}