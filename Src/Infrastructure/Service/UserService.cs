using App.Common;
using App.Repository;
using App.Service;
using App.Validators;
using Domain.Common;
using Domain.Entity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IUserValidator _validator;
        private readonly IHasher _hasher;

        public UserService( ILogger<UserService> logger,  IHasher hasher, IUserValidator validator, IUserRepository userRepository)
        {
            _logger = logger;
            _hasher = hasher;
            _validator = validator; 
            _userRepository = userRepository; 

        }

        public async Task<Result> AddUserAsync(User user)
        {
            // Проверка пароля на соответствие требованиям
            if (! await _validator.IsPasswordStrongAsync(user.Password))
                return Result.Failed("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");

            // Проверка уникальности Email и Login
            if (!await _validator.IsEmailUniqueAsync(user.Email))
                return Result.Failed("This email is already taken.");

            if (!await _validator.IsLoginUniqueAsync(user.Login))
                return Result.Failed("This login is already taken.");

            // Хэширование пароля и сохранение в сущность User
            user.PasswordSalt = _hasher.GenerateSalt();
            user.Password = _hasher.HashPassword(user.Password, user.PasswordSalt);
            

            user.Id = Guid.NewGuid().ToString();
            user.RegistrationDt = DateTime.UtcNow;

            try
            {
                // Добавление пользователя в базу данных
                await _userRepository.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Failed("Something went wrong " + ex.Message);
            }
            return Result.Successful();
        }

      
    }
}
