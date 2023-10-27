﻿using Application.Common.Models;
using Application.Quotes.Commands.CreateQuote;
using Application.Quotes.Queries.GetQuoteById;
using Application.Quotes.Queries.GetQuotesByToast;
using Application.Quotes.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class QuoteController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<QuoteDto> CreateQuote(CreateQuoteCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/toast")]
    public async Task<PaginatedList<QuoteDto>> GetQuotesByToast
        ([FromQuery] GetQuotesByToastQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/id")]
    public async Task<QuoteDto> GetQuoteById
        ([FromQuery] GetQuoteByIdQuery command)
    {
        return await Mediator.Send(command);
    }
}