namespace Domain.Events;

public class UserCreateEvent : BaseEvent
{
    public UserCreateEvent(User item)
    {
        Item = item;
    }
    
    public User Item { get; }
}