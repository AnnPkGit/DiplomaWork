using Application.Common.Mappings;
using Application.Common.Mappings.Actions;
using Application.MediaItems.Queries.Models;
using Application.Quotes.Queries.Models;
using Application.Replies.Queries.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.BaseToasts.Queries.Models;

public class BaseToastWithContentBriefDto : BaseToastBriefDto, IMapFrom<BaseToastWithContentDto>
{
    public string Content { get; set; } = string.Empty;
    public bool YouReacted { get; set; }
    public bool YouReToasted { get; set; }
    public int ReactionsCount { get; set; }
    public int RepliesCount { get; set; }
    public int ReToastsCount { get; set; }
    public int QuotesCount { get; set; }
    public IEnumerable<BaseMediaItemDto> MediaItems { get; set; } = new List<BaseMediaItemDto>();
    
    public virtual void Mapping(Profile profile)
    {
        profile.CreateMap<BaseToastWithContent, BaseToastWithContentBriefDto>()
            .Include<Toast, ToastBriefDto>()
            .Include<Reply, ReplyBriefDto>()
            .Include<Quote, QuoteBriefDto>()
            .AfterMap<SetBaseToastWithContentDtoAction>();
    }
}