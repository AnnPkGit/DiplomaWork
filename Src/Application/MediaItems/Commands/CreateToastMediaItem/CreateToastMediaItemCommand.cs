using Application.Common.Interfaces;
using Application.Common.Security;
using Application.MediaItems.Commands.Common;
using Domain.Entities;
using MediatR;

namespace Application.MediaItems.Commands.CreateToastMediaItem;

[Authorize]
public class CreateToastMediaItemCommand : CreateBaseMediaItemModel , IRequest<CreateBaseMediaItemDto>
{
    public CreateToastMediaItemCommand()
    {
    }

    public CreateToastMediaItemCommand(Stream file, string name, string type) : base(file, name, type)
    {
    }
}

public class CreateToastMediaItemCommandHandler : IRequestHandler<CreateToastMediaItemCommand, CreateBaseMediaItemDto>
{
    private readonly IMediaStorage _mediaService;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;

    public CreateToastMediaItemCommandHandler(
        IMediaStorage mediaService,
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _mediaService = mediaService;
        _context = context;
        _userService = userService;
    }

    public async Task<CreateBaseMediaItemDto> Handle(CreateToastMediaItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var newName = Guid.NewGuid() + "." + request.Type[(request.Type.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
        var url = await _mediaService.UploadMediaAsync(request.File, newName, cancellationToken);
        var mediaItem = new ToastMediaItem(url, newName, request.Type, userId);
        
        await _context.ToastMediaItems.AddAsync(mediaItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateBaseMediaItemDto(mediaItem.Id, url);
    }
}