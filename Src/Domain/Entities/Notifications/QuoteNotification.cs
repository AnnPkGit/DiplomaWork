namespace Domain.Entities.Notifications;

public class QuoteNotification : BaseNotification
{
    public QuoteNotification()
    {
    }

    public QuoteNotification(int toAccountId, int quoteId, DateTime created)
        : base(toAccountId, typeof(QuoteNotification), created)
    {
        QuoteId = quoteId;
    }

    public int QuoteId { get; set; }
    public Quote Quote { get; set; } = null!;
}