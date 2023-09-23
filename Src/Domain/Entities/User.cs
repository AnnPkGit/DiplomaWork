namespace Domain.Entities;

public class User : BaseEntity
{
    public string Email
    {
        get => _email;
        set
        {
            if (_email.Equals(value)) return;
            AddDomainEvent(new UserEmailIsSetEvent(this));
            _email = value;
        }
    }

    public bool EmailVerified
    {
        get => _emailVerified;
        set
        {
            if (_emailVerified.Equals(value)) return;
            AddDomainEvent(new UserEmailVerifiedIsSetEvent(this));
            _emailVerified = value;
        }
    }
    public int? EmailVerifyCode { get; set; }
    public string? Phone { get; set; }
    public bool PhoneVerified { get; set; }
    public int? PhoneVerifyCode { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public int MaxAccountsCount { get; set; }
    public ICollection<Account> Accounts { get; private set; } = new List<Account>();
    
    public ICollection<Role> Roles { get; private set; } = new List<Role>();

    private string _email = null!;

    private bool _emailVerified;
}