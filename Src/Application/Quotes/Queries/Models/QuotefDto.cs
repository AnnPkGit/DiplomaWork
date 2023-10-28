using Application.BaseToasts.Queries.Models;
using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using Domain.Common;
using Domain.Entities;

namespace Application.Quotes.Queries.Models;

public class QuoteDto : BaseToastWithContentDto, IMapFrom<Quote>
{
    public QuotedToastDto QuotedToast { get; set; } = null!;
}

public class QuotedToastDto : BaseToastDto, IMapFrom<BaseToastWithContent>
{
    public string Content { get; set; } = string.Empty;
    public IEnumerable<MediaItemBriefDto> MediaItems { get; set; } = null!;
}