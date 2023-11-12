using Domain.Entities;

namespace Application.Common.Services;

public interface IMediaService
{
    public Task<ToastMediaItem[]> GetToastMediaItemsAsync(CancellationToken cancellationToken, params int[] toastMediaItemIds);
}