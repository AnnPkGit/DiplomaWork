using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Users.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommandValidation : AbstractValidator<UpdateUserPasswordCommand>
{
    private const int MinimumPasswordLenght = CommonUserValidationRules.MinimumPasswordLenght;
    private const int MaximumPasswordLenght = CommonUserValidationRules.MaximumPasswordLenght;
    
    private readonly IHasher _hasher;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public UpdateUserPasswordCommandValidation(IHasher hasher, IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _hasher = hasher;
        _context = context;
        _currentUserService = currentUserService;

        RuleFor(v => v.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(MinimumPasswordLenght)
            .MaximumLength(MaximumPasswordLenght)
            .Matches(CommonUserValidationRules.PasswordRegex).WithMessage(CommonUserValidationRules.PassRegexErrStr)
            .MustAsync(BeNewPasswordUnequalOldOne).WithMessage("The new password must be different from the old one");
    }
    
    private async Task<bool> BeNewPasswordUnequalOldOne(string password, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);
        if (user == null)
            throw new NotFoundException(nameof(User), userId);
        var newPassHash = _hasher.HashPassword(password, user.PasswordSalt);
        return newPassHash != user.Password;
    }
}