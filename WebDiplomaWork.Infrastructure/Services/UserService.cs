using CommonLibrary;
using Microsoft.Extensions.Logging;
using WebDiplomaWork.App;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess;

namespace WebDiplomaWork.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity, string> _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<UserEntity, string> userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result> AddUserAsync(UserEntity user)
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

        private Result CheckForEmailLoginDups(UserEntity user)
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
