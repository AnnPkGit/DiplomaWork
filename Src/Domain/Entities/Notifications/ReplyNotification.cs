namespace Domain.Entities.Notifications;

public class ReplyNotification : BaseNotification
{
    public ReplyNotification()
    {
    }

    public ReplyNotification(int toAccountId, int replyId, DateTime created)
        : base(toAccountId, typeof(ReplyNotification), created)
    {
        ReplyId = replyId;
    }

    public int ReplyId { get; set; }
    public Reply Reply { get; set; } = null!;
}