using System.ComponentModel.DataAnnotations;
using WebDiplomaWork.Helper.Attribute;

namespace WebDiplomaWork.DTO
{
    public class UserDto
    {
        [Required]
        [MinLength(3)]
        public String Login { get; set; }

        public DateTime RegistrationDt { get; set; }

        [Required]
        [Date(ErrorMessage = "Wrong birth date format")]
        public DateTime BirthDate { get; set; }

        public String? Name { get; set; }

        public String? Avatar { get; set; }

        public String? Bio { get; set; }
    }
}
