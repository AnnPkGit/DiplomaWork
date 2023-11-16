using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Common.Services;

public class MediaService : IMediaService
{
    private readonly IApplicationDbContext _context;

    public MediaService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ToastMediaItem[]> GetToastMediaItemsAsync(CancellationToken cancellationToken, params int[] toastMediaItemIds)
    {
        var maxToastMediaItems = toastMediaItemIds.Length;
        var toastMediaItems = new ToastMediaItem[maxToastMediaItems];
        
        for (var i = 0; i < maxToastMediaItems; i++)
        {
            var itemId = toastMediaItemIds[i];
            var item = await _context.ToastMediaItems.FindAsync(new object[] { itemId }, cancellationToken);

            toastMediaItems[i] = item ?? throw new NotFoundException(nameof(ToastMediaItem), itemId);
        }

        return toastMediaItems;
    }
}