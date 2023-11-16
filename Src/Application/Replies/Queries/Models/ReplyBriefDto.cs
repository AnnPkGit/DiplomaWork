using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Replies.Queries.Models;

public class ReplyBriefDto : BaseToastWithContentDto, IMapFrom<Reply>
{
    public BaseToastWithContentDto ReplyToToast { get; set; } = null!;

    public ReplyBriefDto()
    {
    }

    public ReplyBriefDto(ICurrentUserService userService) : base(userService) {}
    
    
    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Reply, ReplyBriefDto>();
        base.Mapping(profile);
    }
}