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

public class BaseToastWithContentDto : BaseToastDto, IMapFrom<BaseToastWithContent>
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
        profile.CreateMap<BaseToastWithContent, BaseToastWithContentDto>()
            .Include<Toast, ToastDto>()
            .Include<Reply, ReplyDto>()
            .Include<Quote, QuoteDto>()
            .AfterMap<SetBaseToastWithContentDtoAction>();
    }
}