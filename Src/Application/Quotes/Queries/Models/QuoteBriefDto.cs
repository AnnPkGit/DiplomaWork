using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Quotes.Queries.Models;

public class QuoteBriefDto : BaseToastWithContentBriefDto, IMapFrom<Quote>
{
    public QuotedToastDto QuotedToast { get; set; } = null!;

    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Quote, QuoteBriefDto>();
        base.Mapping(profile);
    }
}