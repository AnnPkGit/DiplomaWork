using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.UserEmailConfirm;

public record UserEmailConfirmCommand(int UserId, string EmailVerifyToken) : IRequest;

public class UserEmailConfirmCommandHandler : IRequestHandler<UserEmailConfirmCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenValidator _tokenValidator;

    public UserEmailConfirmCommandHandler(
        IApplicationDbContext context,
        ITokenValidator tokenValidator)
    {
        _context = context;
        _tokenValidator = tokenValidator;
    }

    public async Task Handle(UserEmailConfirmCommand request, CancellationToken token)
    {
        if (_tokenValidator.ValidateEmailVerifyToken(request.EmailVerifyToken, out var claims) && request.UserId.Equals(claims.userId))
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(user => user.Id == claims.userId, token);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), claims.userId);
            }
            if (user.EmailVerified)
            {
                var err = new ValidationException();
                err.Errors.Add("EmailVerification", new []{"Your email has already been confirmed"});
                throw err;
            }
            if (!user.Email.Equals(claims.email))
            {
                var err = new ValidationException();
                err.Errors.Add("EmailVerification", new []{"Invalid verification data"});
                throw err;
            }

            user.EmailVerified = true;

            await _context.SaveChangesAsync(token);
        }
        else
        {
            var err = new ValidationException();
            err.Errors.Add("EmailVerification", new []{"Invalid verification data"});
            throw err;
        }
    }
}