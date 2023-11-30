using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Replies.Queries.Models;

public class ReplyDto : BaseToastWithContentDto, IMapFrom<Reply>
{
    public BaseToastWithContentDto ReplyToToast { get; set; } = null!;
    
    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Reply, ReplyDto>();
        base.Mapping(profile);
    }
}