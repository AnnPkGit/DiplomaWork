using App.Helper;
using App.Repository;
using App.Service;
using AutoMapper;
using Domain.Common;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IUserRepository userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = _mapper.Map<User>(user);
            var result = await _userService.AddUserAsync(userEntity);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            
            var result = await _userService.AddUserAsync(userEntity);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }
        
        public async Task<Result> AddUserAsync(UserRegistrationDto userDto)
        {
            // Проверка пароля на соответствие требованиям
            if (! await _userRepository.IsPasswordStrongAsync(userDto.Password))
                return Result.Failed("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");

            // Проверка уникальности Email и Login
            if (!await _userRepository.IsEmailUniqueAsync(userDto.Email))
                return Result.Failed("This email is already taken.");
        
            if (!await _userRepository.IsLoginUniqueAsync(userDto.Login))
                return Result.Failed("This login is already taken.");

            // Конвертация данных из DTO в сущность User
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Login = userDto.Login,
                RegistrationDt = DateTime.UtcNow,
                BirthDate = userDto.BirthDate,
                Name = userDto.Login,
                Email = userDto.Email,
                EmailVerified = false,
                Phone = null,
                PhoneVerified = false,
                Avatar = null,
                Bio = null
            };
            
            // Хэширование пароля и сохранение в сущность User
            user.PasswordSalt = _hasher.GenerateSalt();
            user.Password = _hasher.HashPassword(userDto.Password, user.PasswordSalt);

            // Добавление пользователя в базу данных
            await _userRepository.AddUserAsync(user);
            return Result.Successful();
        }

    }
    
    
    
}