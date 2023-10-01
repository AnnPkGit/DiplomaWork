using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;

namespace Application.Auth.Commands.SendVerifyMsgByEmail;

[Authorize]
public record SendVerifyMsgByEmailCommand : IRequest;

public class SendVerifyMsgByEmailCommandHandler : IRequestHandler<SendVerifyMsgByEmailCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailConfirmationSender _emailSender;

    public SendVerifyMsgByEmailCommandHandler(
        ICurrentUserService currentUserService,
        IEmailConfirmationSender emailSender)
    {
        _currentUserService = currentUserService;
        _emailSender = emailSender;
    }

    public async Task Handle(SendVerifyMsgByEmailCommand request, CancellationToken token)
    {
        if(_currentUserService.EmailVerified is not null && !_currentUserService.EmailVerified.Value)
        {
            await _emailSender.SendAsync(new EmailConfirmRequest(_currentUserService.Id, _currentUserService.Email), token);
        }
    }
}