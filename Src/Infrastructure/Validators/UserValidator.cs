using System.Text.RegularExpressions;
using App.Repository;
using App.Validators;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validators;

public class UserValidator : IUserValidator
{
    private readonly IUserRepository _userRepository;

    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        // Проверка уникальности Email
        return await _userRepository.GetAll().AllAsync(u => u.Email != email);
    }

    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        // Проверка уникальности Login
        return await _userRepository.GetAll().AllAsync(u => u.Login != login);
    }

    public Task<bool> IsPasswordStrongAsync(string password)
    {
        // Проверка сложности pass
        return Task.Run(() => IsPasswordStrong(password));
    }

    public bool IsPasswordStrong(string password)
    {
        const string regexPattern = @"^(?=.*\p{Lu})(?=.*\p{Ll})(?=.*\d)(?=.*[^\p{L}\p{N}]).{8,}$";

        return Regex.IsMatch(password, regexPattern);
    }
}