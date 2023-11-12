using Application.MediaItems.Commands.Common;
using Application.MediaItems.Commands.CreateAvatarMediaItem;
using Application.MediaItems.Commands.CreateToastMediaItem;
using Application.MediaItems.Queries.GetAllMediaItems;
using Application.MediaItems.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

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
    
    [HttpGet("all")]
    public async Task<IEnumerable<BaseMediaItemDto>> GetAllMediaItems
        ([FromQuery]  GetAllMediaItemsQuery query)
    {
        return await Mediator.Send(query);
    }
}