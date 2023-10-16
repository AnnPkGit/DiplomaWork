using Domain.Entities;

namespace Application.Common.Services;

public interface IMediaService
{
    public Task<MediaItem[]> GetMediaItemsAsync(CancellationToken cancellationToken, params int[] mediaItemIds);
}