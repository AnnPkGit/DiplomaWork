namespace Domain.Events;

public class UserEmailVerifiedIsSetEvent : BaseEvent
{
    public UserEmailVerifiedIsSetEvent(User item)
    {
        Item = item;
    }
    
    public User Item { get; }
}