using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.UpdateUserPhone;

public class UpdateUserPhoneCommandValidator : AbstractValidator<UpdateUserPhoneCommand>
{
    private const int PhoneLenght = CommonUserValidationRules.PhoneLenght;

    private readonly IApplicationDbContext _context;
    public UpdateUserPhoneCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(v => v.NewPhone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(PhoneLenght)
            .Matches(@"^\d+$").WithMessage("Phone must contain only digits")
            .MustAsync(BeUniquePhoneAsync).WithMessage("This phone is already taken");
    }

    private async Task<bool> BeUniquePhoneAsync(string phone, CancellationToken token)
    {
        return await _context.Users.AllAsync(u => u.Phone != phone, token);
    }
}