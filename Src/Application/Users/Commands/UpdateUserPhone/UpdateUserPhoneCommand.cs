using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.UpdateUserPhone;

[Authorize]
public record UpdateUserPhoneCommand(string NewPhone) : IRequest;

public class UpdateUserPhoneCommandHandler : IRequestHandler<UpdateUserPhoneCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserPhoneCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task Handle(UpdateUserPhoneCommand request, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);

        if (user == null)
            throw new NotFoundException(nameof(User), userId);

        user.Phone = request.NewPhone;
        
        await _context.SaveChangesAsync(token);
    }
}