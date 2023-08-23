using Domain.Common;

namespace Domain.Entity;

public class User : BaseEntity
{
    public DateTime RegistrationDt { get; set; }
    public string Email { get; set; }
    public bool EmailVerified { get; set; }
    public string? Phone { get; set; }
    public bool PhoneVerified { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public int MaxAccountsCount { get; set; }
    public IList<Account> Accounts { get; private set; } = new List<Account>();
}