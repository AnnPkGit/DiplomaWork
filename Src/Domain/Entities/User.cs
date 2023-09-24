namespace Domain.Entities;

public class User : BasicLegalEntity
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
    public string Password { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    
    public Account? Account { get; set; }
    public ICollection<Role> Roles { get; private set; } = new List<Role>();

    private string _email = null!;

    private bool _emailVerified;
}