using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Toasts.Commands.CreateReToast;

[Authorize]
public record CreateReToastCommand(int ToastId) : IRequest<ToastBriefDto>;

public class CreateReToastCommandHandler : IRequestHandler<CreateReToastCommand, ToastBriefDto>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public CreateReToastCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IMapper mapper,
        IDateTime dateTime)
    {
        _userService = userService;
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<ToastBriefDto> Handle(CreateReToastCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var newReToast = new ReToast(request.ToastId, userId, _dateTime.Now);

        var toast = await _context.Toasts.FindAsync(new object?[] { request.ToastId }, cancellationToken);
        if (toast == null)
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        _context.ReToasts.Add(newReToast);
        await _context.SaveChangesAsync(cancellationToken);
            
        toast.Created = newReToast.Created;
        toast.Type = "reToast";
        
        return _mapper.Map<ToastBriefDto>(toast);
    }
}