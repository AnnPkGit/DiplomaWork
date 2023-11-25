using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Domain.Entities.Notifications;

namespace Application.BaseNotifications.Queries.Models;

public class FollowerNotificationDto : BaseNotificationDto, IMapFrom<FollowerNotification>
{
    public AccountBriefDto Follower { get; set; } = null!;
}