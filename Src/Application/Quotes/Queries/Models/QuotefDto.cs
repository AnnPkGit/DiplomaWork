using Application.BaseToasts.Queries.Models;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.Quotes.Queries.Models;

public class QuoteDto : BaseToastWithContentDto, IMapFrom<Quote>
{
    public QuotedToastDto QuotedToast { get; set; } = null!;
    
    public override void Mapping(Profile profile)
    {
        profile.CreateMap<Quote, QuoteDto>();
        base.Mapping(profile);
    }
}

public class QuotedToastDto : BaseToastDto, IMapFrom<BaseToastWithContent>
{
    public string Content { get; set; } = string.Empty;
    public IEnumerable<BaseMediaItemDto> MediaItems { get; set; } = null!;
}