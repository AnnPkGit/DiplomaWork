namespace Domain.Entity;

public class User
{
    public virtual String Id { get; set;  }
    public virtual String Login { get; set; }
    public virtual DateTime RegistrationDt { get; set; }
    public virtual DateTime BirthDate { get; set; }
    public virtual String? Name { get; set; }
    public virtual String Email { get; set; }
    public virtual bool EmailVerified { get; set; }
    public virtual String? Phone { get; set; }
    public virtual bool PhoneVerified { get; set; }
    public virtual String? Avatar { get; set; }
    public virtual String? Bio { get; set; }
    public virtual string Password { get; set; }
    public virtual string PasswordSalt { get; set; }

}