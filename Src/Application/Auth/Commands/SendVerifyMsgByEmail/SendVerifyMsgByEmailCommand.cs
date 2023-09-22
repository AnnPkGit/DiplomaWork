using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;

namespace Application.Auth.Commands.SendVerifyMsgByEmail;

[Authorize]
public record SendVerifyMsgByEmailCommand : IRequest;

public class SendVerifyMsgByEmailCommandHandler : IRequestHandler<SendVerifyMsgByEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly ITokenProvider _tokenProvider;
    private readonly ICurrentUserService _currentUserService;

    public SendVerifyMsgByEmailCommandHandler(
        IEmailService emailService,
        ITokenProvider tokenProvider,
        ICurrentUserService currentUserService)
    {
        _emailService = emailService;
        _tokenProvider = tokenProvider;
        _currentUserService = currentUserService;
    }

    public async Task Handle(SendVerifyMsgByEmailCommand request, CancellationToken token)
    {
        var emailVerified = _currentUserService.EmailVerified;
        if(emailVerified is not null && !emailVerified.Value)
        {
            var user = (_currentUserService.Id, _currentUserService.Email);
            const string hostname = "http://localhost:5000";
            const string confirmPageUrl = "/api/v1/auth/confirmation/email";
            var emailVerifyToken = _tokenProvider.GetEmailVerifyToken(user.Id, user.Email);
            var body =
                $"<p>Follow <a href=\"{hostname}{confirmPageUrl}?id={user.Id}&token={emailVerifyToken}\">this link</a> to confirm your email</p>";
        
            await _emailService.SendEmailAsync(new MailRequest
            {
                Subject = "no-reply",
                ToEmail = user.Email,
                Body = body
            }, token);
        }
    }
}