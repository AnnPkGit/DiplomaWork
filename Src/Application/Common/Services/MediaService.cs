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

    public async Task<MediaItem[]> GetMediaItemsAsync(CancellationToken cancellationToken, params int[] mediaItemIds)
    {
        var maxMediaItems = mediaItemIds.Length;
        var mediaItems = new MediaItem[maxMediaItems];
        
        for (var i = 0; i < maxMediaItems; i++)
        {
            var itemId = mediaItemIds[i];
            var item = await _context.MediaItems.FindAsync(new object[] { itemId }, cancellationToken);

            mediaItems[i] = item ?? throw new NotFoundException(nameof(MediaItem), itemId);
        }

        return mediaItems;
    }
}