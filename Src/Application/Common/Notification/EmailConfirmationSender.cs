using Application.Common.Configurations.Options;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Common.Notification;
public class EmailConfirmationSender : IEmailConfirmationSender
{
    private readonly IEmailService _emailService;
    private readonly ITokenProvider _tokenProvider;
    private readonly EmailConfirmationSenderOptions _options;

    public EmailConfirmationSender(
        IEmailService emailService,
        ITokenProvider tokenProvider,
        IOptionsSnapshot<EmailConfirmationSenderOptions> options)
    {
        _emailService = emailService;
        _tokenProvider = tokenProvider;
        _options = options.Value;
    }

    public Task SendAsync(EmailConfirmRequest request, CancellationToken token = default)
    {
        var confirmPageUrl = _options.ConfirmPageUrl;
        var linkContent = _options.LinkContent;
        var body = _options.Body;

        if (!_options.CheckRequiredValues())
        {
            throw new FormatException("EmailConfirmationSenderOptions is not configured");
        }
        
        var emailVerifyToken = _tokenProvider.GetEmailVerifyToken(request.Id, request.Email);
        var linkTag = $"<a href=\"{confirmPageUrl}?id={request.Id}&token={emailVerifyToken}\">{linkContent}</a>";
        body = body.Replace(EmailConfirmationSenderOptions.LINK_CONTENT, linkTag);
        
        return _emailService.SendEmailAsync(new MailRequest
        {
            Subject = "no-reply",
            ToEmail = request.Email,
            Body = body
        }, token);
    }
}