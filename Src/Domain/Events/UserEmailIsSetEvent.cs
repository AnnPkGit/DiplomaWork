namespace Domain.Events;

public class UserEmailIsSetEvent : BaseEvent
{
    public UserEmailIsSetEvent(User item)
    {
        Item = item;
    }
    
    public User Item { get; }
}