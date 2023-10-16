using Application.Accounts.Queries.Models;
using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ToastBriefDto : IMapFrom<Toast>
{
    public int Id { get; set; }

    public DateTime? LastModified { get; set; }
    public DateTime Created { get; set; }
    public AccountBriefDto? Author { get; set; }
    public string Context { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    
    public ReplyBriefDto? Reply { get; set; }
    
    public QuoteBriefDto? Quote { get; set; }
    public int ReactionCount { get; set; }
    public int ReToastCount { get; set; }
    public int ReplyCount { get; set; }
    public bool IsReToast { get; set; }
    public ICollection<MediaItemBriefDto>? MediaItems { get; set; }
    public ICollection<ReplyBriefDto>? Thread { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Toast, ToastBriefDto>()
            .ForMember(dto => dto.ReactionCount, opt => opt
                .MapFrom(t => t.Reactions == null ? 0 : t.Reactions.Count))
            .ForMember(dto => dto.ReToastCount, opt => opt
                .MapFrom(t => t.ReToasters == null
                    ? t.Quotes == null
                        ? 0
                        : t.Quotes.Count
                    : t.Quotes == null
                        ? t.ReToasters.Count
                        : t.ReToasters.Count + t.Quotes.Count))
            .ForMember(dto => dto.ReplyCount, opt => opt
                .MapFrom(t => t.Replies == null ? 0 : t.Replies.Count))
        .ForMember(dto => dto.Thread, opt => opt
            .MapFrom((t, dto, value, ctx) => t.Replies?
                .Where(r => r.AuthorId == t.AuthorId)
                .OrderBy(r => r.Created)
                .Select(r => ctx.Mapper.Map<ReplyBriefDto>(r))
            ));
    }
}