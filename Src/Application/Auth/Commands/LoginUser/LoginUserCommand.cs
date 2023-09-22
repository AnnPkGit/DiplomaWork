using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponse>;

public record LoginUserResponse(string AccessToken, string RefreshToken);

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IHasher _hasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IApplicationDbContext _context;

    public LoginUserCommandHandler(
        ITokenProvider tokenProvider,
        IApplicationDbContext context,
        IHasher hasher)
    {
        _tokenProvider = tokenProvider;
        _context = context;
        _hasher = hasher;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken token)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.Email == request.Email, token);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), request.Email);
        }
            
        var requestPass = _hasher.HashPassword(request.Password, user.PasswordSalt);
        if (user.Password != requestPass)
        {
            throw new ValidationException("Password is not correct");
        }

        var accessToken = _tokenProvider.GenAccessToken(user);
        var refreshToken = _tokenProvider.GenRefreshToken();
        return new LoginUserResponse(accessToken, refreshToken);
    }
}