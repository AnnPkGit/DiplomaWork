using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Queries.Models;

public class UserBriefDto : IMapFrom<User>
{
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public AccountDto? Account { get; set; }
}