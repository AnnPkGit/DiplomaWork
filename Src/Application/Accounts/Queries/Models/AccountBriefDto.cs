using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using Domain.Entities;

namespace Application.Accounts.Queries.Models;

public class AccountBriefDto : IMapFrom<Account>
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public AvatarMediaItemDto? Avatar { get; set; }
    public string? Bio { get; set; }
}