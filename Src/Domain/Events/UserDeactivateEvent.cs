namespace Domain.Events;

public class UserDeactivateEvent : BaseEvent
{
    public UserDeactivateEvent(User item)
    {
        Item = item;
    }

    public User Item { get; }
}