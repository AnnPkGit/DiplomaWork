using Application.Common.Models;
using Application.Replies.Commands.CreateReply;
using Application.Replies.Queries.GetRepliesByToast;
using Application.Replies.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class ReplyController : ApiV1ControllerBase
{
    
    [HttpPost, Authorize]
    public async Task<ReplyBriefDto> CreateReply(CreateReplyCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/toast")]
    public async Task<PaginatedList<ReplyBriefDto>> GetRepliesByToast
        ([FromQuery] GetRepliesByToastQuery command)
    {
        return await Mediator.Send(command);
    }
}