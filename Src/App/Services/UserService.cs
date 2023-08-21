using App.Common.Interfaces;
using App.Common.Interfaces.Services;
using App.Common.Interfaces.Validators;
using Domain.Common;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Services;

public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResponse(string AccessToken, string RefreshToken);
public sealed class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserValidator _validator;
    private readonly IHasher _hasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IApplicationDbContext _dbContext;

    public UserService(
        ILogger<UserService> logger,
        IHasher hasher,
        IUserValidator validator,
        ITokenProvider tokenProvider,
        IApplicationDbContext dbContext)
    {
        _logger = logger;
        _hasher = hasher;
        _validator = validator; 
        _tokenProvider = tokenProvider;
        _dbContext = dbContext;
    }

    public async Task<Result> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        // Проверка пароля на соответствие требованиям
        if (! await _validator.IsPasswordStrongAsync(user.Password))
            return Result.Failed("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");

        // Проверка уникальности Email и Login
        if (!await _validator.IsEmailUniqueAsync(user.Email))
            return Result.Failed("This email is already taken.");

        // if (!await _validator.IsLoginUniqueAsync(user.Login))
        //     return Result.Failed("This login is already taken.");

        // Хэширование пароля и сохранение в сущность User
        user.PasswordSalt = _hasher.GenerateSalt();
        user.Password = _hasher.HashPassword(user.Password, user.PasswordSalt);
            

        user.Id = Guid.NewGuid();
        user.RegistrationDt = DateTime.Now.ToUniversalTime();

        try
        {
            // Добавление пользователя в базу данных
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Failed("Something went wrong " + ex.Message);
        }
        return Result.Successful();
    }

    public async Task<Result<LoginResponse>> LoginUserAsync(LoginRequest request, CancellationToken token)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == request.Email,
                cancellationToken: token);
        if (user is null)
        {
            return Result<LoginResponse>.Failed("User not found");
        }
            
        var requestPass = _hasher.HashPassword(request.Password, user.PasswordSalt);
        if (user.Password != requestPass)
        {
            return Result<LoginResponse>.Failed("Password is not correct");
        }

        var accessToken = _tokenProvider.GenAccessToken(user);
        var refreshToken = _tokenProvider.GenRefreshToken();

        return Result<LoginResponse>.Successful(new (accessToken, refreshToken))!;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Users.ToListAsync(cancellationToken);
    }
}