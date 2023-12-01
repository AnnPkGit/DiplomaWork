using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Users.Queries.Models;

public class MuteNotificationOptionDto : IMapFrom<MuteNotificationOption>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}