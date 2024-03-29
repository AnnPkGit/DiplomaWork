﻿using Application.Common.Mappings;
using Application.ReToasts.Queries.Models;
using Domain.Entities.Notifications;

namespace Application.BaseNotifications.Queries.Models;

public class ReToastNotificationDto : BaseNotificationDto, IMapFrom<ReToastNotification>
{
    public ReToastDto ReToast { get; set; } = null!;
}