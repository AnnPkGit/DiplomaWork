using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Users.Queries.GetCurrentUser;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.LoginUserUsingSession;

public record LoginUserUsingSessionCommand(string Email, string Password) : IRequest<UserBriefDto>;

public class LoginUserUsingSessionCommandHandler : IRequestHandler<LoginUserUsingSessionCommand, UserBriefDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IHasher _hasher;
    private readonly IMapper _mapper;
    
    public LoginUserUsingSessionCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        IHasher hasher, IMapper mapper)
    {
        _currentUserService = currentUserService;
        _context = context;
        _hasher = hasher;
        _mapper = mapper;
    }

    public async Task<UserBriefDto> Handle(LoginUserUsingSessionCommand request, CancellationToken token)
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
            var err = new ValidationException();
            err.Errors.Add("Password", new[] { "Password is not correct" });
            throw err;
        }

        await _currentUserService.SetIdAsync(user.Id, cancellationToken: token);
        await _currentUserService.SetRolesAsync(user.Roles, cancellationToken: token);
        await _currentUserService.SetEmailAsync(user.Email, cancellationToken: token);
        await _currentUserService.SetEmailVerifiedAsync(user.EmailVerified, cancellationToken: token);
        return _mapper.Map<UserBriefDto>(user);
    }
}