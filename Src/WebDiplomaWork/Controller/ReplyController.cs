﻿using Application.Common.Models;
using Application.Replies.Commands.CreateReply;
using Application.Replies.Queries.GetAllReplies;
using Application.Replies.Queries.GetRepliesByToast;
using Application.Replies.Queries.GetReplyById;
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
    
    [HttpGet("all")]
    public async Task<PaginatedList<ReplyBriefDto>> GetAllReplies
        ([FromQuery] GetAllRepliesQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/toast")]
    public async Task<PaginatedList<ReplyBriefDto>> GetRepliesByToast
        ([FromQuery] GetRepliesByToastQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/id")]
    public async Task<ReplyDto> GetReplyById
        ([FromQuery] GetReplyByIdQuery command)
    {
        return await Mediator.Send(command);
    }
}