using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.ReToasts.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ReToasts.Commands.CreateReToast;

[Authorize]
public record CreateReToastCommand(int ToastWithContentId) : IRequest<ReToastDto>;

public class CreateReToastCommandHandler : IRequestHandler<CreateReToastCommand, ReToastDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public CreateReToastCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IDateTime dateTime,
        IMapper mapper)
    {
        _context = context;
        _userService = userService;
        _dateTime = dateTime;
        _mapper = mapper;
    }

    public async Task<ReToastDto> Handle(CreateReToastCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var toastWithContentId = request.ToastWithContentId;
        var toastWithContent = await _context
            .BaseToastsWithContent
            .Include(toast => toast.Reactions)
            .Include(toast => toast.ReToasts)
            .Include(toast => toast.Replies)
            .Include(toast => toast.Quotes)
            .SingleOrDefaultAsync( toast => toast.Id == toastWithContentId , cancellationToken);

        if (toastWithContent == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }
        
        switch (toastWithContent)
        {
            case Reply entity:
                await _context.Replies
                    .Entry(entity)
                    .Reference(reply => reply.ReplyToToast)
                    .LoadAsync(cancellationToken);
                break;
            case Quote entity:
                await _context.Quotes
                    .Entry(entity)
                    .Reference(quote => quote.QuotedToast)
                    .LoadAsync(cancellationToken);
                break;
        }
        
        if (await _context.ReToasts.AnyAsync(r =>
                r.ToastWithContentId == toastWithContentId &&
                r.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var createDate = _dateTime.Now;
        var newReToast = new ReToast(accountId, toastWithContentId);
        
        await _context.ReToasts.AddAsync(newReToast, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        if (res != 0 && toastWithContent.AuthorId != accountId)
        {
            var newReToastNotification = new ReToastNotification(toastWithContent.AuthorId, newReToast.Id, createDate);
            await _context.BaseNotifications.AddAsync(newReToastNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return _mapper.Map<ReToastDto>(newReToast);
    }
}