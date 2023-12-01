namespace Application.Common.Interfaces;

public interface IMuteNotificationOptionsChecker
{
    public Task<bool> CheckMuteOptions(int fromAccountId, int toAccountId, CancellationToken cancellationToken = default);
}