using System.ComponentModel.DataAnnotations;

namespace WebDiplomaWork.DTO;

public sealed class LoginRquestDto
{
    [Required]
    [MinLength(3)]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}