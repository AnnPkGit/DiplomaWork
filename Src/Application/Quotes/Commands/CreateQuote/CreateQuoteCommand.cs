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
    private readonly IMuteNotificationOptionsChecker _optionsChecker;

    public CreateQuoteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IMapper mapper,
        IMediaService mediaService,
        IDateTime dateTime,
        IMuteNotificationOptionsChecker optionsChecker)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
        _mediaService = mediaService;
        _dateTime = dateTime;
        _optionsChecker = optionsChecker;
    }

    public async Task<QuoteDto> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var quotedToastId = request.QuotedToastId;
        var quotedToast = await _context.BaseToastsWithContent.FindAsync(new object?[] { quotedToastId }, cancellationToken);
        
        if (quotedToast == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), quotedToastId);
        }
        
        var fromAccountId = _userService.Id;
        var toAccountId = quotedToast.Author == null ? null : quotedToast.AuthorId;
        
        var mediaItems = _mediaService.GetToastMediaItemsAsync(cancellationToken, request.ToastMediaItemIds);

        var newQuote = new Quote(fromAccountId, request.Content, quotedToastId, await mediaItems);
        var createDate = _dateTime.Now;
        
        await _context.Quotes.AddAsync(newQuote, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        if (res != 0 && toAccountId != fromAccountId &&
            await _optionsChecker.CheckMuteOptions(fromAccountId, toAccountId, cancellationToken))
        {
            var newQuoteNotification = new QuoteNotification(toAccountId!.Value, newQuote.Id, createDate);
            await _context.BaseNotifications.AddAsync(newQuoteNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return _mapper.Map<QuoteDto>(newQuote);
    }
}