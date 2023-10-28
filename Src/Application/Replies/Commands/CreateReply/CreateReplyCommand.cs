using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Replies.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Replies.Commands.CreateReply;

[Authorize]
public record CreateReplyCommand(string Content, int ReplyToToastId, params int[] MediaItemIds) : IRequest<ReplyBriefDto>;

public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, ReplyBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CreateReplyCommandHandler(
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

    public async Task<ReplyBriefDto> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var replyToToastId = request.ReplyToToastId;
        
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == replyToToastId, cancellationToken))
        {
            throw new NotFoundException(nameof(BaseToastWithContent), replyToToastId);
        }
        
        var mediaItems = _mediaService.GetMediaItemsAsync(cancellationToken, request.MediaItemIds);
        
        var newReply = new Reply(accountId, request.Content, replyToToastId, await mediaItems);

        await _context.Replies.AddAsync(newReply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReplyBriefDto>(newReply);
    }
}
