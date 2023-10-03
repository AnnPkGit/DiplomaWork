namespace Infrastructure.Persistence.RelationshipTables;

public class ToastReaction
{
    public int ToastId { get; set; }
    public int AccountId { get; set; }
    public int ReactionId { get; set; }
    public DateTime Reacted { get; set; }
}