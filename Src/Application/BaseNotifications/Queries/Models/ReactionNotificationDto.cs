using Application.Common.Mappings;
using Application.Reactions.Queries.Models;
using Domain.Entities.Notifications;

namespace Application.BaseNotifications.Queries.Models;

public class ReactionNotificationDto : BaseNotificationDto, IMapFrom<ReactionNotification>
{
    public ReactionBriefDto Reaction { get; set; } = null!;
}