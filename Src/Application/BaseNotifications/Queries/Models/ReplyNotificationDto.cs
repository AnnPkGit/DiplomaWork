using Application.Common.Mappings;
using Application.Replies.Queries.Models;
using Domain.Entities.Notifications;

namespace Application.BaseNotifications.Queries.Models;

public class ReplyNotificationDto : BaseNotificationDto, IMapFrom<ReplyNotification>
{
    public ReplyBriefDto Reply { get; set; } = null!;
}