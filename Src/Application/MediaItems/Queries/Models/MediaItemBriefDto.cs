using Application.Common.Mappings;
using Domain.Entities;

namespace Application.MediaItems.Queries.Models;

public class MediaItemBriefDto : IMapFrom<MediaItem>
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}