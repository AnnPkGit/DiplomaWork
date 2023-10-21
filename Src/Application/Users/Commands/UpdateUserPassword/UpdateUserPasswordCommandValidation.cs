using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
        
        RuleFor(command => command.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotEqual(command => command.OldPassword)
            .MinimumLength(MinimumPasswordLenght)
            .MaximumLength(MaximumPasswordLenght)
            .Matches(CommonUserValidationRules.PasswordRegex)
            .WithMessage(CommonUserValidationRules.PassRegexErrStr)
            .DependentRules(() =>
            {
                RuleFor(command => command.OldPassword)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .MinimumLength(MinimumPasswordLenght)
                    .MaximumLength(MaximumPasswordLenght)
                    .Matches(CommonUserValidationRules.PasswordRegex)
                    .WithMessage(CommonUserValidationRules.PassRegexErrStr)
                    .MustAsync(CheckOldPassword).WithMessage("The old password is incorrect");
            });
    }

    private async Task<bool> CheckOldPassword(string oldPassword, CancellationToken token)
    {
        var userId = _currentUserService.Id;

        var userHashes = await _context.Users.Where(user => user.Id == userId)
            .Select(user => new {user.Password , user.PasswordSalt}).SingleOrDefaultAsync(token);
        if (userHashes == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }
        
        var oldPassHash = _hasher.HashPassword(userHashes.Password, userHashes.PasswordSalt);
        return oldPassHash == userHashes.Password;
    }
}