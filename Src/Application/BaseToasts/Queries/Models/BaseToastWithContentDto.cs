using Application.Common.Interfaces;
using Application.Common.Mappings;
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
    private readonly ICurrentUserService? _userService;

    public BaseToastWithContentDto()
    {
    }

    public BaseToastWithContentDto(ICurrentUserService userService)
    {
        _userService = userService;
    }

    public string Content { get; set; } = string.Empty;
    public bool YouReacted { get; set; }
    public bool YouReToasted { get; set; }
    public int ReactionsCount { get; set; }
    public int RepliesCount { get; set; }
    public int ReToastsCount { get; set; }
    public int QuotesCount { get; set; }
    public IEnumerable<MediaItemBriefDto> MediaItems { get; set; } = null!;

    public virtual void Mapping(Profile profile)
    {
        profile.CreateMap<BaseToastWithContent, BaseToastWithContentDto>()
            .Include<Toast, ToastBriefDto>()
            .Include<Reply, ReplyDto>()
            .Include<Reply, ReplyBriefDto>()
            .Include<Quote, QuoteDto>()
            .ForMember(dto => dto.YouReacted, expression => expression
                .MapFrom(content => content.Reactions
                    .Any(reaction => _userService != null && reaction.AuthorId == _userService.Id)))
            .ForMember(dto => dto.YouReToasted, expression => expression
                .MapFrom(content => content.ReToasts
                    .Any(reToast => _userService != null && reToast.AuthorId == _userService.Id)));
    }
}