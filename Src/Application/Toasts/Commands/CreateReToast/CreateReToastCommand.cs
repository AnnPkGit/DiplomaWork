using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Commands.CreateReToast;

[Authorize]
public record CreateReToastCommand(int ToastId) : IRequest<ToastBriefDto>;

public class CreateReToastCommandHandler : IRequestHandler<CreateReToastCommand, ToastBriefDto>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReToastCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IMapper mapper)
    {
        _userService = userService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ToastBriefDto> Handle(CreateReToastCommand request, CancellationToken token)
    {
        var userId = _userService.Id;

        var toast = await _context.Toasts.SingleOrDefaultAsync(t => t.Id == request.ToastId && t.Type != ToastType.ReToast, token);
        if (toast == null)
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var newReToast = Toast.CreateReToast(userId, request.ToastId);
        _context.Toasts.Add(newReToast);
        await _context.SaveChangesAsync(token);
            
        newReToast.ReToast = toast;
        
        return _mapper.Map<ToastBriefDto>(newReToast);
    }
}