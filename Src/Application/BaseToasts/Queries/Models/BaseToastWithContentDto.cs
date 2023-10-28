using Application.Common.Mappings;
using Application.MediaItems.Queries.Models;
using Domain.Common;

namespace Application.BaseToasts.Queries.Models;

public class BaseToastWithContentDto : BaseToastDto, IMapFrom<BaseToastWithContent>
{
    public string Content { get; set; } = string.Empty;
    public bool YouReacted { get; set; }
    public bool YouReToasted { get; set; }
    public int ReactionsCount { get; set; }
    public int RepliesCount { get; set; }
    public int ReToastsCount { get; set; }
    public int QuotesCount { get; set; }
    public IEnumerable<MediaItemBriefDto> MediaItems { get; set; } = null!;
}