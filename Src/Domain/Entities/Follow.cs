namespace Domain.Entities;

public class Follow
{
    public Follow() { }

    public Follow(int followFromId, int followToId, DateTime dateOfFollow)
    {
        FollowFromId = followFromId;
        FollowToId = followToId;
        DateOfFollow = dateOfFollow;
    }

    public int Id { get; set; }
    public int FollowFromId { get; set; }
    public Account FollowFrom { get; set; } = null!;
    public int FollowToId { get; set; }
    public Account FollowTo { get; set; } = null!;
    public DateTime DateOfFollow { get; set; }
}
