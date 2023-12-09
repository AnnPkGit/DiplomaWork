using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Replies.Queries.Models;

public class ReplyDto : BaseToastWithContentDto, IMapFrom<Reply>
{
    public BaseToastDto ReplyToToast { get; set; } = null!;
    
    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Reply, ReplyDto>()
            .ForMember(dto => dto.ReplyToToast, expression => expression
                .MapFrom((reply, _, _, context) => reply.ReplyToToast == null
                    ? new DeletedBaseToastDto()
                    : (BaseToastDto)context.Mapper.Map<BaseToastWithContentDto>(reply.ReplyToToast)));
        base.Mapping(profile);
    }
}