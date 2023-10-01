namespace Application.Common.Interfaces;

public record EmailConfirmRequest(int Id, string Email);

public interface IEmailConfirmationSender
{
    public Task SendAsync(EmailConfirmRequest request, CancellationToken cancellationToken = default);
}