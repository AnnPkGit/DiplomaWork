using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.LoginUserUsingSession;

public record LoginUserUsingSessionCommand(string Email, string Password) : IRequest;

public class LoginUserUsingSessionCommandHandler : IRequestHandler<LoginUserUsingSessionCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IHasher _hasher;
    
    public LoginUserUsingSessionCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        IHasher hasher)
    {
        _currentUserService = currentUserService;
        _context = context;
        _hasher = hasher;
    }

    public async Task Handle(LoginUserUsingSessionCommand request, CancellationToken token)
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

        await _currentUserService.SetIdAsync(user.Id, cancellationToken: token);
        await _currentUserService.SetRolesAsync(user.Roles, cancellationToken: token);
        await _currentUserService.SetEmailAsync(user.Email, cancellationToken: token);
        await _currentUserService.SetEmailVerifiedAsync(user.EmailVerified, cancellationToken: token);
    }
}