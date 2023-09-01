using System.Text.RegularExpressions;
using App.Common.Exceptions;
using App.Common.Interfaces;
using App.Common.Interfaces.Validators;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validators;

public class UserValidator : IUserValidator
{
    private readonly IHasher _hasher;
    private readonly IApplicationDbContext _dbContext;

    public UserValidator(
        IHasher hasher,
        IApplicationDbContext dbContext)
    {
        _hasher = hasher;
        _dbContext = dbContext;
    }
    
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        // Проверка уникальности Email
        return await _dbContext.Users.AllAsync(u => u.Email != email);
    }

    public Task<bool> IsPasswordStrongAsync(string password)
    {
        // Проверка сложности pass
        return Task.Run(() => IsPasswordStrong(password));
    }

    public async Task<bool> IsNewPasswordUnequalAsync(int id, string password, CancellationToken token)
    {
        var user = await _dbContext.Users.FindAsync(new object?[] { id }, token);
        if (user == null)
            throw new NotFoundException("User", id);
        var newPassHash = _hasher.HashPassword(password, user.PasswordSalt);
        return newPassHash != user.Password;
    }

    public async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken token)
    {
        return await _dbContext.Users.AllAsync(u => u.Phone != phone, token);
    }

    public bool IsPasswordStrong(string password)
    {
        const string regexPattern = @"^(?=.*\p{Lu})(?=.*\p{Ll})(?=.*\d)(?=.*[^\p{L}\p{N}]).{8,}$";

        return Regex.IsMatch(password, regexPattern);
    }
}