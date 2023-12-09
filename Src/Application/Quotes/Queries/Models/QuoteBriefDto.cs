using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Quotes.Queries.Models;

public class QuoteBriefDto : BaseToastWithContentBriefDto, IMapFrom<Quote>
{
    public BaseToastDto QuotedToast { get; set; } = null!;

    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Quote, QuoteBriefDto>()
            .ForMember(dto => dto.QuotedToast, expression => expression
                .MapFrom((quote, _, _, context) => quote.QuotedToast == null
                    ? new DeletedBaseToastDto()
                    : (BaseToastDto)context.Mapper.Map<QuotedToastDto>(quote.QuotedToast)));
        base.Mapping(profile);
    }
}