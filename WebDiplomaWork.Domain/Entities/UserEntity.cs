using System.ComponentModel.DataAnnotations.Schema;

namespace WebDiplomaWork.Domain.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        public String Id { get; set;  }

        public String Login { get; set; }

        public DateTime RegistrationDt { get; set; }

        public DateTime BirthDate { get; set; }

        public String? Name { get; set; }

        public String Email { get; set; }

        public int EmailVerified { get; set; }

        public String? Phone { get; set; }

        public int PhoneVerified { get; set; }

        public String? Avatar { get; set; }

        public String? Bio { get; set; }
    }
}
