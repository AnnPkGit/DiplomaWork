using System.Text.RegularExpressions;
using App.Validators;
using Infrastructure.DbAccess.EfDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validators;

public class UserValidator : IUserValidator
{
    private readonly DataContext _dbContext;

    public UserValidator(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        // Проверка уникальности Email
        return await _dbContext.Users.AllAsync(u => u.Email != email);
    }

    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        // Проверка уникальности Login
        return await _dbContext.Users.AllAsync(u => u.Login != login);
    }

    public Task<bool> IsPasswordStrongAsync(string password)
    {
        // Проверка сложности pass
        return Task.Run(() =>
        {
            string regexPattern = @"^(?=.*\p{Lu})(?=.*\p{Ll})(?=.*\d)(?=.*[^\p{L}\p{N}]).{8,}$";

            if (!Regex.IsMatch(password, regexPattern))
                return false;

            return true;
        });
    }
}