using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Quotes.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quotes.Commands.CreateQuote;

[Authorize]
public record CreateQuoteCommand(string Content, int QuotedToastId, params int[] MediaItemIds) : IRequest<QuoteDto>;

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, QuoteDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CreateQuoteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IMapper mapper,
        IMediaService mediaService)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    public async Task<QuoteDto> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var quotedToastId = request.QuotedToastId;
        
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == quotedToastId, cancellationToken))
        {
            throw new NotFoundException(nameof(BaseToastWithContent), quotedToastId);
        }
        if (await _context.Quotes.AnyAsync(q => q.QuotedToastId == quotedToastId && q.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }
        
        var mediaItems = _mediaService.GetMediaItemsAsync(cancellationToken, request.MediaItemIds);

        var newQuote = new Quote(accountId, request.Content, quotedToastId, await mediaItems);

        await _context.Quotes.AddAsync(newQuote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuoteDto>(newQuote);
    }
}