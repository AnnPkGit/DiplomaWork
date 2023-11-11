namespace Domain.Entities.Notifications;

public class ReToastNotification : BaseNotification
{
    public ReToastNotification()
    {
    }

    public ReToastNotification(int toAccountId, int reToastId, DateTime created)
        : base(toAccountId, typeof(ReToastNotification), created)
    {
        ReToastId = reToastId;
    }

    public int ReToastId { get; set; }
    public ReToast ReToast { get; set; } = null!;
}