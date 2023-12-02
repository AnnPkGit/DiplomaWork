using Application.Common.Interfaces;
using Application.MediaItems.Commands.Common;
using Domain.Entities;
using MediatR;

namespace Application.MediaItems.Commands.CreateBannerMediaItem;

public class CreateBannerMediaItemCommand : CreateBaseMediaItemModel, IRequest<CreateBaseMediaItemDto>
{
    public CreateBannerMediaItemCommand()
    {
    }

    public CreateBannerMediaItemCommand(Stream file, string name, string type) : base(file, name, type)
    {
    }
}

public class CreateBannerMediaItemCommandHandler : IRequestHandler<CreateBannerMediaItemCommand, CreateBaseMediaItemDto>
{
    private readonly IMediaStorage _mediaStorage;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    
    public CreateBannerMediaItemCommandHandler(
        IMediaStorage mediaStorage,
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _mediaStorage = mediaStorage;
        _context = context;
        _userService = userService;
    }

    public async Task<CreateBaseMediaItemDto> Handle(CreateBannerMediaItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var newName = Guid.NewGuid() + "." + request.Type[(request.Type.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
        var url = await _mediaStorage.UploadMediaAsync(request.File, newName, cancellationToken);
        var mediaItem = new BannerMediaItem(url, newName, request.Type, userId);
        
        await _context.BannerMediaItems.AddAsync(mediaItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateBaseMediaItemDto(mediaItem.Id, url);
    }
}