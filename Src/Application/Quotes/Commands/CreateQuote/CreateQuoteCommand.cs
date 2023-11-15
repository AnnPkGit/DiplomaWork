using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Quotes.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quotes.Commands.CreateQuote;

[Authorize]
public record CreateQuoteCommand(string Content, int QuotedToastId, params int[] ToastMediaItemIds) : IRequest<QuoteDto>;

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, QuoteDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;
    private readonly IDateTime _dateTime;

    public CreateQuoteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IMapper mapper,
        IMediaService mediaService,
        IDateTime dateTime)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
        _mediaService = mediaService;
        _dateTime = dateTime;
    }

    public async Task<QuoteDto> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var quotedToastId = request.QuotedToastId;
        var quotedToast = await _context.BaseToastsWithContent.FindAsync(new object?[] { quotedToastId }, cancellationToken);
        
        if (quotedToast == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), quotedToastId);
        }
        
        if (await _context.Quotes.AnyAsync(q => q.QuotedToastId == quotedToastId && q.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }
        
        var mediaItems = _mediaService.GetToastMediaItemsAsync(cancellationToken, request.ToastMediaItemIds);

        var newQuote = new Quote(accountId, request.Content, quotedToastId, await mediaItems);
        var createDate = _dateTime.Now;
        
        await _context.Quotes.AddAsync(newQuote, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        if (res != 0 && quotedToast.AuthorId != accountId)
        {
            var newQuoteNotification = new QuoteNotification(quotedToast.AuthorId, newQuote.Id, createDate);
            await _context.BaseNotifications.AddAsync(newQuoteNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return _mapper.Map<QuoteDto>(newQuote);
    }
}