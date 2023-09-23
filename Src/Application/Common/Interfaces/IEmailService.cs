namespace Application.Common.Interfaces;

public class MailRequest
{
    public string ToEmail { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}

public interface IEmailService
{
    public Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default);
}