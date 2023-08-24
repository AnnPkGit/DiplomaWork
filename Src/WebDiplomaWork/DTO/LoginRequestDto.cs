using System.ComponentModel.DataAnnotations;

namespace WebDiplomaWork.DTO;

public sealed class LoginRequestDto
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Wrong email format")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}