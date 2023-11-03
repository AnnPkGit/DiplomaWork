using Application.Accounts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Common;
using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseNotifications.Queries.Models;

public class BaseNotificationDto : IMapFrom<BaseNotification>
{
    public int Id { get; set; }
    public AccountBriefDto ToAccount { get; set; } = null!;
    public string Type { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Viewed { get; set; }

    public static IEnumerable<BaseNotificationDto> ToBaseNotificationDto(BaseNotification[] baseNotifications, IApplicationDbContext context, IMapper mapper)
    {
        var reactionNotifyIds = baseNotifications
            .Where(bn => bn.Type == nameof(ReactionNotification))
            .Select(rn => rn.Id).ToArray();
        var quoteNotifyIds = baseNotifications
            .Where(bn => bn.Type == nameof(QuoteNotification))
            .Select(qn => qn.Id).ToArray();
        var replyNotifyIds = baseNotifications
            .Where(bn => bn.Type == nameof(ReplyNotification))
            .Select(rn => rn.Id).ToArray();
        var reToastNotifyIds = baseNotifications
            .Where(bn => bn.Type == nameof(ReToastNotification))
            .Select(rtn => rtn.Id).ToArray();
        
        var objectsDto = new List<BaseNotificationDto>(baseNotifications.Length);
        if (reactionNotifyIds.Any())
        {
            var reactionNotifications = context.ReactionNotifications
                .Include(rn => rn.Reaction).ThenInclude(r => r.ToastWithContent)
                .Where(rn => reactionNotifyIds.Contains(rn.Id));
            objectsDto.AddRange(reactionNotifications.Select(rn => mapper.Map<ReactionNotificationDto>(rn)));
        }
        if (quoteNotifyIds.Any())
        {
            var quoteNotifications = context.QuoteNotifications
                .Include(rn => rn.Quote).ThenInclude(q => q.Reactions)
                .Include(rn => rn.Quote).ThenInclude(q => q.Replies)
                .Include(rn => rn.Quote).ThenInclude(q => q.Quotes)
                .Include(rn => rn.Quote).ThenInclude(q => q.ReToasts)
                .Where(rn => quoteNotifyIds.Contains(rn.Id));
            objectsDto.AddRange(quoteNotifications.Select(rn => mapper.Map<QuoteNotificationDto>(rn)));
        }
        if (replyNotifyIds.Any())
        {
            var replyNotifications = context.ReplyNotifications
                .Include(rn => rn.Reply).ThenInclude(r => r.Reactions)
                .Include(rn => rn.Reply).ThenInclude(r => r.Replies)
                .Include(rn => rn.Reply).ThenInclude(r => r.Quotes)
                .Include(rn => rn.Reply).ThenInclude(r => r.ReToasts)
                .Where(rn => replyNotifyIds.Contains(rn.Id));
            objectsDto.AddRange(replyNotifications.Select(rn => mapper.Map<ReplyNotificationDto>(rn)));
        }
        if (reToastNotifyIds.Any())
        {
            var reToastNotifications = context.ReToastNotifications
                .Include(rn => rn.ReToast).ThenInclude(rt => rt.ToastWithContent)
                .Where(rn => reToastNotifyIds.Contains(rn.Id));
            objectsDto.AddRange(reToastNotifications.Select(rn => mapper.Map<ReToastNotificationDto>(rn)));
        }

        return objectsDto;
    }
}