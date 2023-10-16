using Application.Common.Models;
using Application.MediaItems.Commands.CreateMediaItem;
using Application.MediaItems.Queries.GetMediaItemsWithPagination;
using Application.MediaItems.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class MediaItemController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<CreateMediaItemDto> CreateMedia(IFormFile file)
    {
        return await Mediator.Send(new CreateMediaItemCommand(file.OpenReadStream(), file.FileName, file.ContentType));
    }
    
    [HttpGet("pagination"), Authorize]
    public async Task<ActionResult<PaginatedList<MediaItemBriefDto>>> GetMediaItemsWithPagination
        ([FromQuery]  GetMediaItemsWithPaginationQuery command)
    {
        return await Mediator.Send(command);
    }
}