using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entity;

namespace Infrastructure.DbAccess.Entity;

[Table("Users")]
public class UserEntity : User
{
    public override String Id { get; set;  }
    public override String Login { get; set; }
    public override DateTime RegistrationDt { get; set; }
    public override DateTime BirthDate { get; set; }
    public override String? Name { get; set; }
    public override String Email { get; set; }
    public override int EmailVerified { get; set; }
    public override String? Phone { get; set; }
    public override int PhoneVerified { get; set; }
    public override String? Avatar { get; set; }
    public override String? Bio { get; set; }
    public override string Password { get; set; }
    public override string PasswordSalt { get; set; }
}