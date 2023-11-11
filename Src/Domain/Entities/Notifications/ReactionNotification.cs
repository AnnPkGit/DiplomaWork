namespace Domain.Entities.Notifications;

public class ReactionNotification : BaseNotification
{
    public ReactionNotification()
    {
    }

    public ReactionNotification(int toAccountId, int reactionId, DateTime created)
        : base(toAccountId, typeof(ReactionNotification), created)
    {
        ReactionId = reactionId;
    }

    public int ReactionId { get; set; }
    public Reaction Reaction { get; set; } = null!;
}