using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Accounts.Queries.Models;

public class AccountBriefDto : IMapFrom<Account>
{
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}