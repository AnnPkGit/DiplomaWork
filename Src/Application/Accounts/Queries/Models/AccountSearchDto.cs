using Application.Common.Mappings;
using Application.Common.Mappings.Actions;
using Application.MediaItems.Queries.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Accounts.Queries.Models;

public class AccountSearchDto : IMapFrom<Account>
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string? Name { get; set; }
    public AvatarMediaItemDto? Avatar { get; set; }
    public string? Bio { get; set; }
    public bool YouFollow { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Account, AccountSearchDto>()
            .AfterMap<SetAccountSearchDtoAction>();
    }
}