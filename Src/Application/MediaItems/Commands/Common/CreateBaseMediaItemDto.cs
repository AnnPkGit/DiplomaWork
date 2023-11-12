using Application.Common.Mappings;
using Domain.Common;

namespace Application.MediaItems.Commands.Common;

public class CreateBaseMediaItemDto : IMapFrom<BaseMediaItem>
{
    public CreateBaseMediaItemDto()
    {
    }

    public CreateBaseMediaItemDto(int id, string url)
    {
        Id = id;
        Url = url;
    }

    public int Id { get; init; }
    public string Url { get; init; } = string.Empty;
}