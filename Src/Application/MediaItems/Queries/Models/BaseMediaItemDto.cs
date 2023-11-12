using Application.Common.Mappings;
using Domain.Common;

namespace Application.MediaItems.Queries.Models;

public class BaseMediaItemDto : IMapFrom<BaseMediaItem>
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}