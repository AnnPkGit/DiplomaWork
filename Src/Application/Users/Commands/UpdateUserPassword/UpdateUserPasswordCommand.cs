using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.UpdateUserPassword;

[Authorize]
public record UpdateUserPasswordCommand(string OldPassword, string NewPassword) : IRequest;

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHasher _hasher;

    public UpdateUserPasswordCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IHasher hasher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _hasher = hasher;
    }

    public async Task Handle(UpdateUserPasswordCommand request, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);
        
        if (user == null)
            throw new NotFoundException(nameof(User), userId);

        var newSalt = _hasher.GenerateSalt();
        var newPassHash = _hasher.HashPassword(request.NewPassword, newSalt);

        user.PasswordSalt = newSalt;
        user.Password = newPassHash;

        await _context.SaveChangesAsync(token);
    }
}