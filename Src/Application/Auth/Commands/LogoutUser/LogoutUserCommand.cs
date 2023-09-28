using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;

namespace Application.Auth.Commands.LogoutUser;

[Authorize]
public record LogoutUserCommand: IRequest;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    private readonly ICurrentUserService _currentUserService;

    public LogoutUserCommandHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.Clear(cancellationToken);
    }
}