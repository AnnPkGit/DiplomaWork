using App.Repository;
using App.Service;
using Domain.Common;
using Domain.Entity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, string> _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<User, string> userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result> AddUserAsync(User user)
        {
            // TODO: добавить парольхэш и пароль-соль в UserEntity 
            // Хэшер уже зарегестрирван в Program (WebDiplomaWork.Infrastructure->Services->Helpers->IHasher)

            var result = CheckForEmailLoginDups(user);
            if(!result.IsSuccessful)
                return result;

            user.Id = Guid.NewGuid().ToString();
            user.RegistrationDt = DateTime.UtcNow;

            try
            {
                await _userRepository.CreateAsync(user);
                await _userRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Failed("Something went wrong");
            }

            return Result.Successful();
        }

        private Result CheckForEmailLoginDups(User user)
        {
            var emailDup = _userRepository.GetAll().Where(u => u.Email.Equals(user.Email)).FirstOrDefault();
            if (emailDup != null)
                return Result.Failed("This email is already taken");

            var loginDup = _userRepository.GetAll().Where(u => u.Login.Equals(user.Login)).FirstOrDefault();
            if (loginDup != null)
                return Result.Failed("This login is already taken");

            return Result.Successful();
        }
    }
}
