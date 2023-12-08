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
        var followerNotifyIds = baseNotifications
            .Where(bn => bn.Type == nameof(FollowerNotification))
            .Select(rtn => rtn.Id).ToArray();
        
        var objectsDto = new List<BaseNotificationDto>(baseNotifications.Length);
        if (reactionNotifyIds.Any())
        {
            var reactionNotifications = context.ReactionNotifications
                .IgnoreAutoIncludes()
                .Include(rn => rn.Reaction)
                    .ThenInclude(r => r.ToastWithContent)
                    .ThenInclude(t => t.Author)
                .Include(rn => rn.Reaction)
                    .ThenInclude(r => r.Author)
                    .ThenInclude(a => a.Avatar)
                .Where(rn => reactionNotifyIds.Contains(rn.Id))
                .AsSingleQuery()
                .ToArray();
            objectsDto.AddRange(reactionNotifications.Select(mapper.Map<ReactionNotificationDto>));
        }
        if (quoteNotifyIds.Any())
        {
            var quoteNotifications = context.QuoteNotifications
                .IgnoreAutoIncludes()
                .Include(rn => rn.Quote)
                    .ThenInclude(q => q.Author)
                    .ThenInclude(a => a.Avatar)
                .Include(rn => rn.Quote).ThenInclude(q => q.MediaItems)
                .Include(rn => rn.Quote).ThenInclude(q => q.Reactions)
                .Include(rn => rn.Quote).ThenInclude(q => q.Replies)
                .Include(rn => rn.Quote).ThenInclude(q => q.Quotes)
                .Include(rn => rn.Quote).ThenInclude(q => q.ReToasts)
                .Include(rn => rn.Quote)
                    .ThenInclude(q => q.QuotedToast)
                        .ThenInclude(t => t!.Author)
                        .ThenInclude(a => a.Avatar)
                .Include(rn => rn.Quote)
                    .ThenInclude(q => q.QuotedToast)
                        .ThenInclude(t => t!.MediaItems)
                .Where(rn => quoteNotifyIds.Contains(rn.Id))
                .AsSingleQuery()
                .ToArray();
            objectsDto.AddRange(quoteNotifications.Select(mapper.Map<QuoteNotificationDto>));
        }
        if (replyNotifyIds.Any())
        {
            var replyNotifications = context.ReplyNotifications
                .IgnoreAutoIncludes()
                .Include(rn => rn.Reply)
                    .ThenInclude(q => q.Author)
                    .ThenInclude(a => a.Avatar)
                .Include(rn => rn.Reply).ThenInclude(r => r.MediaItems)
                .Include(rn => rn.Reply).ThenInclude(r => r.Reactions)
                .Include(rn => rn.Reply).ThenInclude(r => r.Replies)
                .Include(rn => rn.Reply).ThenInclude(r => r.Quotes)
                .Include(rn => rn.Reply).ThenInclude(r => r.ReToasts)
                .Include(rn => rn.Reply)
                    .ThenInclude(r => r.ReplyToToast).ThenInclude(t => t!.Author)
                .Where(rn => replyNotifyIds.Contains(rn.Id))
                .AsSingleQuery()
                .ToArray();
            objectsDto.AddRange(replyNotifications.Select(mapper.Map<ReplyNotificationDto>));
        }
        if (reToastNotifyIds.Any())
        {
            var reToastNotifications = context.ReToastNotifications
                .IgnoreAutoIncludes()
                .Include(rn => rn.ReToast)
                    .ThenInclude(rt => rt.ToastWithContent)
                    .ThenInclude(t => t!.Author)
                        .ThenInclude(a => a.Avatar)
                .Where(rn => reToastNotifyIds.Contains(rn.Id))
                .AsSingleQuery()
                .ToArray();
            objectsDto.AddRange(reToastNotifications.Select(mapper.Map<ReToastNotificationDto>));
        }
        if (followerNotifyIds.Any())
        {
            var followerNotifications = context.FollowerNotifications
                .IgnoreAutoIncludes()
                .Include(fn => fn.Follower).ThenInclude(a => a.Avatar)
                .Where(fn => followerNotifyIds.Contains(fn.Id))
                .AsSingleQuery()
                .ToArray();
            objectsDto.AddRange(followerNotifications.Select(mapper.Map<FollowerNotificationDto>));
        }

        return objectsDto;
    }
}