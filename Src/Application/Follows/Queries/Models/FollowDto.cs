using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Follows.Queries.Models;

public class FollowDto : IMapFrom<Follow>
{
    public int Id { get; set; }
    public int AccountId { get; set; } // Id того, кто подписывается
    public int ToAccountId { get; set; } // Id того, на кого подписываются
    public DateTime DateOfFollow { get; set; } // Дата та час підписки

}