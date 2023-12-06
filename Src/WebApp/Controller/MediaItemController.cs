using Application.MediaItems.Commands.Common;
using Application.MediaItems.Commands.CreateAvatarMediaItem;
using Application.MediaItems.Commands.CreateBannerMediaItem;
using Application.MediaItems.Commands.CreateToastMediaItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

public class MediaItemController : ApiV1ControllerBase
{
    [HttpPost("toast"), Authorize]
    public async Task<CreateBaseMediaItemDto> CreateToastMediaItem(IFormFile file)
    {
        return await Mediator.Send(new CreateToastMediaItemCommand(file.OpenReadStream(), file.FileName, file.ContentType));
    }
    
    [HttpPost("avatar"), Authorize]
    public async Task<CreateBaseMediaItemDto> CreateAvatarMediaItem(IFormFile file)
    {
        return await Mediator.Send(new CreateAvatarMediaItemCommand(file.OpenReadStream(), file.FileName, file.ContentType));
    }
    
    [HttpPost("banner"), Authorize]
    public async Task<CreateBaseMediaItemDto> CreateBannerMediaItem(IFormFile file)
    {
        return await Mediator.Send(new CreateBannerMediaItemCommand(file.OpenReadStream(), file.FileName, file.ContentType));
    }
}