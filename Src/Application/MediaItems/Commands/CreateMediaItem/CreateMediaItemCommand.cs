using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.MediaItems.Commands.CreateMediaItem;

[Authorize]
public record CreateMediaItemCommand(Stream File, string Name, string Type) : IRequest<CreateMediaItemDto>;

public class CreateMediaItemCommandHandler : IRequestHandler<CreateMediaItemCommand, CreateMediaItemDto>
{
    private readonly IMediaService _mediaService;
    private readonly IApplicationDbContext _context;

    public CreateMediaItemCommandHandler(
        IMediaService mediaService,
        IApplicationDbContext context)
    {
        _mediaService = mediaService;
        _context = context;
    }

    public async Task<CreateMediaItemDto> Handle(CreateMediaItemCommand request, CancellationToken cancellationToken)
    {
        var url = await _mediaService.UploadMediaAsync(request.File, request.Name, cancellationToken);
        var mediaItem = new MediaItem(url, request.Name, request.Type);
        
        await _context.MediaItems.AddAsync(mediaItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateMediaItemDto(mediaItem.Id, url);
    }
}