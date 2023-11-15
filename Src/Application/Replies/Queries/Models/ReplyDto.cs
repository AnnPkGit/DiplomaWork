using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Replies.Queries.Models;

public class ReplyDto : BaseToastWithContentDto, IMapFrom<Reply>
{
    public BaseToastWithContentDto ReplyToToast { get; set; } = null!;

    public ReplyDto()
    {
    }

    public override void Mapping(Profile profile)
    {
        var destinationType = typeof(BaseToastWithContentDto);
        profile.CreateMap<Reply, ReplyDto>();
        base.Mapping(profile);
    }


    public ReplyDto(ICurrentUserService userService) : base(userService) {}
}