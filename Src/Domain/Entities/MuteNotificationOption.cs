namespace Domain.Entities;

public class MuteNotificationOption : Enumeration<MuteNotificationOption>
{
    public static readonly MuteNotificationOption YouDoNotFollow = new(1, nameof(YouDoNotFollow));
    public static readonly MuteNotificationOption WhoDoNotFollowYou = new(2, nameof(WhoDoNotFollowYou));
    public static readonly MuteNotificationOption WithANewAccount = new(3, nameof(WithANewAccount));
    public static readonly MuteNotificationOption WhoHaveDefaultProfilePhoto = new(4, nameof(WhoHaveDefaultProfilePhoto));
    public static readonly MuteNotificationOption WhoHaveNotConfirmedTheirEmail = new(5, nameof(WhoHaveNotConfirmedTheirEmail));
    public static readonly MuteNotificationOption WhoHaveNotConfirmedTheirPhoneNumber = new(6, nameof(WhoHaveNotConfirmedTheirPhoneNumber));
    public MuteNotificationOption(int id, string name) : base(id, name)
    {
    }
}