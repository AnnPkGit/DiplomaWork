using Application.Common.Mappings;
using Application.Quotes.Queries.Models;
using Domain.Entities.Notifications;

namespace Application.BaseNotifications.Queries.Models;

public class QuoteNotificationDto : BaseNotificationDto, IMapFrom<QuoteNotification>
{
    public QuoteDto Quote { get; set; } = null!;
}