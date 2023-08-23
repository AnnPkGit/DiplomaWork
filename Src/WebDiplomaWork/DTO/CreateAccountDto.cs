using System.ComponentModel.DataAnnotations;

namespace WebDiplomaWork.DTO;

public sealed class CreateAccountDto
{
    [Required]
    public string Login { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Avatar { get; set; }
}