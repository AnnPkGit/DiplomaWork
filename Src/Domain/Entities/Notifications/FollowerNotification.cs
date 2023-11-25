namespace Domain.Entities.Notifications;

public class FollowerNotification : BaseNotification
{
    public FollowerNotification()
    {
    }

    public FollowerNotification(int toAccountId, int followerId, DateTime created)
        : base(toAccountId, typeof(FollowerNotification), created)
    {
        FollowerId = followerId;
    }
    
    public int FollowerId { get; set; }
    public Account Follower { get; set; } = null!;
}