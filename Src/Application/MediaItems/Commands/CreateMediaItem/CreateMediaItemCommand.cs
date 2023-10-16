using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.MediaItems.Commands.CreateMediaItem;

[Authorize]
public record CreateMediaItemCommand(Stream File, string Name, string Type) : IRequest<CreateMediaItemDto>;

public class CreateMediaItemCommandHandler : IRequestHandler<CreateMediaItemCommand, CreateMediaItemDto>
{
    private readonly IMediaStorage _mediaService;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;

    public CreateMediaItemCommandHandler(
        IMediaStorage mediaService,
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _mediaService = mediaService;
        _context = context;
        _userService = userService;
    }

    public async Task<CreateMediaItemDto> Handle(CreateMediaItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var url = await _mediaService.UploadMediaAsync(request.File, request.Name, cancellationToken);
        var mediaItem = new MediaItem(url, request.Name, request.Type, userId);
        
        await _context.MediaItems.AddAsync(mediaItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateMediaItemDto(mediaItem.Id, url);
    }
}