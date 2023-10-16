using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Toasts.Commands.CreateReply;

[Authorize]
public record CreateReplyCommand(string Context, int ToastId, params int[] MediaItemIds) : IRequest<ToastBriefDto>;

public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, ToastBriefDto>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CreateReplyCommandHandler(
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

    public async Task<ToastBriefDto> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        
        var mediaItems = _mediaService.GetMediaItemsAsync(cancellationToken, request.MediaItemIds);
        
        var toast = await _context.Toasts.FindAsync(new object?[] { request.ToastId }, cancellationToken);
        if (toast == null)
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var newReply = Toast.CreateReply(userId, request.Context, request.ToastId, await mediaItems);
        
        _context.Toasts.Add(newReply);
        await _context.SaveChangesAsync(cancellationToken);

        newReply.Reply = toast;
        
        return _mapper.Map<ToastBriefDto>(newReply);
    }
}

