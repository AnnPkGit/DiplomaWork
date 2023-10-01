using Application.Common.Interfaces;

namespace Application.Common.Notification;
public class EmailConfirmationSender : IEmailConfirmationSender
{
    private readonly IEmailService _emailService;
    private readonly ITokenProvider _tokenProvider;

    public EmailConfirmationSender(
        IEmailService emailService,
        ITokenProvider tokenProvider)
    {
        _emailService = emailService;
        _tokenProvider = tokenProvider;
    }

    public async Task SendAsync(EmailConfirmRequest request, CancellationToken token = default)
    {
        const string hostname = "http://localhost:5000";
        const string confirmPageUrl = "/api/v1/auth/confirmation/email";
        var emailVerifyToken = _tokenProvider.GetEmailVerifyToken(request.Id, request.Email);
        var body =
            $"<p>Follow <a href=\"{hostname}{confirmPageUrl}?id={request.Id}&token={emailVerifyToken}\">this link</a> to confirm your email</p>";
        
        await _emailService.SendEmailAsync(new MailRequest
        {
            Subject = "no-reply",
            ToEmail = request.Email,
            Body = body
        }, token);
    }
}