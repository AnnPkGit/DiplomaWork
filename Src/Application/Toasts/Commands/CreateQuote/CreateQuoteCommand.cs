using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Toasts.Commands.CreateQuote;

[Authorize]
public record CreateQuoteCommand(string Context, int ToastId, params int[] MediaItemIds) : IRequest<ToastBriefDto>;

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, ToastBriefDto>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CreateQuoteCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IMapper mapper,
        IMediaService mediaService)
    {
        _userService = userService;
        _context = context;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    public async Task<ToastBriefDto> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;

        var mediaItems = _mediaService.GetMediaItemsAsync(cancellationToken, request.MediaItemIds);
        
        var toast = await _context.Toasts.FindAsync(new object?[] { request.ToastId }, cancellationToken);
        if (toast == null)
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var newQuote = Toast.CreateQuote(userId, request.Context, request.ToastId, await mediaItems);
        
        _context.Toasts.Add(newQuote);
        await _context.SaveChangesAsync(cancellationToken);

        newQuote.Quote = toast;
        
        return _mapper.Map<ToastBriefDto>(newQuote);
    }
}