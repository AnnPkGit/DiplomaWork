using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Replies.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using MediatR;

namespace Application.Replies.Commands.CreateReply;

[Authorize]
public record CreateReplyCommand(string Content, int ReplyToToastId, params int[] ToastMediaItemIds) : IRequest<ReplyBriefDto>;

public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, ReplyBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;
    private readonly IDateTime _dateTime;
    private readonly IMuteNotificationOptionsChecker _optionsChecker;

    public CreateReplyCommandHandler(
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

    public async Task<ReplyBriefDto> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
    {
        var replyToToastId = request.ReplyToToastId;
        var replyToToast = await _context.BaseToastsWithContent.FindAsync( new object?[]{ replyToToastId } , cancellationToken);

        if (replyToToast == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), replyToToastId);
        }
        
        var fromAccountId = _userService.Id;
        var toAccountId = replyToToast.AuthorId;
        
        var mediaItems = _mediaService.GetToastMediaItemsAsync(cancellationToken, request.ToastMediaItemIds);
        
        var createDate = _dateTime.Now;
        var newReply = new Reply(fromAccountId, request.Content, replyToToastId, await mediaItems);

        await _context.Replies.AddAsync(newReply, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        
        if (res != 0 &&toAccountId != fromAccountId &&
            await _optionsChecker.CheckMuteOptions(fromAccountId, toAccountId, cancellationToken))
        {
            var newReplyNotification = new ReplyNotification(toAccountId!.Value, newReply.Id, createDate);
            await _context.BaseNotifications.AddAsync(newReplyNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return _mapper.Map<ReplyBriefDto>(newReply);
    }
}