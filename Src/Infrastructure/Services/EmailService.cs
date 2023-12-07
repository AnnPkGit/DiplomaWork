using Application.Common.Interfaces;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptionsSnapshot<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken token)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_emailOptions.Email);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder
        {
            HtmlBody = mailRequest.Body
        };
        email.Body = builder.ToMessageBody();
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls, token);
        await smtp.AuthenticateAsync(_emailOptions.Email, _emailOptions.Password, token);
        await smtp.SendAsync(email, token);
        await smtp.DisconnectAsync(true, token);
    }
}