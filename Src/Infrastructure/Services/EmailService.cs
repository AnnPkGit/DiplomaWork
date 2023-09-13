using Application.Common.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Configuration.ConfigurationManager;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IConfigurationManager configurationManager)
    {
        _emailSettings = configurationManager.EmailSettings;
    }

    public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken token)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_emailSettings.Email);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder
        {
            HtmlBody = mailRequest.Body
        };
        email.Body = builder.ToMessageBody();
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls, token);
        await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password, token);
        await smtp.SendAsync(email, token);
        await smtp.DisconnectAsync(true, token);
    }
}