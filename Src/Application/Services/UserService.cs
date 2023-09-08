using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Validators;
using Domain.Common;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResponse(string AccessToken, string RefreshToken);
public sealed class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserValidator _validator;
    private readonly IHasher _hasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        ILogger<UserService> logger,
        IHasher hasher,
        IUserValidator validator,
        ITokenProvider tokenProvider,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _hasher = hasher;
        _validator = validator; 
        _tokenProvider = tokenProvider;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Result> CreateUserAsync(User user, CancellationToken token)
    {
        // Проверка пароля на соответствие требованиям
        if (! await _validator.IsPasswordStrongAsync(user.Password))
            throw new ValidationException("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");

        // Проверка уникальности Email и Login
        if (!await _validator.IsEmailUniqueAsync(user.Email))
            throw new ValidationException("This email is already taken.");

        // Хэширование пароля и сохранение в сущность User
        user.PasswordSalt = _hasher.GenerateSalt();
        user.Password = _hasher.HashPassword(user.Password, user.PasswordSalt);
        user.RegistrationDt = DateTime.Now.ToUniversalTime();
        user.MaxAccountsCount = 1;

        try
        {
            // Добавление пользователя в базу данных
            await _dbContext.Users.AddAsync(user, token);
            await _dbContext.SaveChangesAsync(token);
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
            throw new NotFoundException("User", request.Email);
        }
            
        var requestPass = _hasher.HashPassword(request.Password, user.PasswordSalt);
        if (user.Password != requestPass)
        {
            throw new ValidationException("Password is not correct");
        }

        var accessToken = _tokenProvider.GenAccessToken(user);
        var refreshToken = _tokenProvider.GenRefreshToken();

        return Result<LoginResponse>.Successful(new (accessToken, refreshToken))!;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token)
    {
        return await _dbContext.Users.ToListAsync(token);
    }

    public async Task<Result> DeleteUserAsync(CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _dbContext.Users.FindAsync(new object?[] { userId }, token);
        if (user == null)
            throw new NotFoundException("User", userId);
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(token);
        
        return Result.Successful();
    }

    public async Task<Result> ChangePasswordAsync(
        string newPassword,
        CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _dbContext.Users.FindAsync(new object?[] { userId }, token);
        
        if (user == null)
            throw new NotFoundException("User", userId);
        
        if (! await _validator.IsPasswordStrongAsync(newPassword))
            throw new ValidationException("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");
        
        if (! await _validator.IsNewPasswordUnequalAsync(userId, newPassword, token))
            throw new ValidationException("The new password must be different from the old one");

        var newSalt = _hasher.GenerateSalt();
        var newPassHash = _hasher.HashPassword(newPassword, newSalt);

        user.PasswordSalt = newSalt;
        user.Password = newPassHash;

        var res = await _dbContext.SaveChangesAsync(token);
        return res <= 0 ? Result.Failed("The context has not changed") : Result.Successful();
    }

    public async Task<Result> ChangeEmailAsync(
        string newEmail,
        CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _dbContext.Users.FindAsync(new object?[] { userId }, token);

        if (user == null)
            throw new NotFoundException("User", userId);
        
        if (!await _validator.IsEmailUniqueAsync(newEmail))
            throw new ValidationException("This email is already taken");
        
        // TODO: Implement phone number verification

        user.Email = newEmail;
        
        var res = await _dbContext.SaveChangesAsync(token);
        return res <= 0 ? Result.Failed("The context has not changed") : Result.Successful();
    }

    public async Task<Result> ChangePhoneAsync(
        string newPhone,
        CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _dbContext.Users.FindAsync(new object?[] { userId }, token);

        if (user == null)
            throw new NotFoundException("User", userId);

        if (!await _validator.IsPhoneUniqueAsync(newPhone, token))
            throw new ValidationException("This phone is already taken");
        
        // TODO: Implement phone number verification
        
        user.Phone = newPhone;
        
        var res = await _dbContext.SaveChangesAsync(token);
        return res <= 0 ? Result.Failed("The context has not changed") : Result.Successful();
    }
}