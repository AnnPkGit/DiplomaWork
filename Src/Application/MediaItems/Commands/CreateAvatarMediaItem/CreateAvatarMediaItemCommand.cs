using Application.Common.Interfaces;
using Application.Common.Security;
using Application.MediaItems.Commands.Common;
using Domain.Entities;
using MediatR;

namespace Application.MediaItems.Commands.CreateAvatarMediaItem;

[Authorize]
public class CreateAvatarMediaItemCommand : CreateBaseMediaItemModel, IRequest<CreateBaseMediaItemDto>
{
    public CreateAvatarMediaItemCommand()
    {
    }

    public CreateAvatarMediaItemCommand(Stream file, string name, string type) : base(file, name, type)
    {
    }
}

public class CreateAvatarMediaItemCommandHandler : IRequestHandler<CreateAvatarMediaItemCommand, CreateBaseMediaItemDto>
{
    private readonly IMediaStorage _mediaStorage;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;

    public CreateAvatarMediaItemCommandHandler(
        IMediaStorage mediaService,
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _mediaStorage = mediaService;
        _context = context;
        _userService = userService;
    }

    public async Task<CreateBaseMediaItemDto> Handle(CreateAvatarMediaItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var newName = Guid.NewGuid() + "." + request.Type[(request.Type.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
        var url = await _mediaStorage.UploadMediaAsync(request.File, newName, cancellationToken);
        var mediaItem = new AvatarMediaItem(url, newName, request.Type, userId);
        
        await _context.AvatarMediaItems.AddAsync(mediaItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateBaseMediaItemDto(mediaItem.Id, url);
    }
}