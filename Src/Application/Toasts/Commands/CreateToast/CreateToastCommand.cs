﻿using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Common.Services;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Toasts.Commands.CreateToast;

[Authorize]
public record CreateToastCommand(string Context, params int[] MediaItemIds) : IRequest<ToastBriefDto>;

public class CreateToastCommandHandler : IRequestHandler<CreateToastCommand, ToastBriefDto>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CreateToastCommandHandler(
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

    public async Task<ToastBriefDto> Handle(CreateToastCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        
        var mediaItems = _mediaService.GetMediaItemsAsync(cancellationToken, request.MediaItemIds);

        var newToast = new Toast(userId, request.Context, await mediaItems);
        
        await _context.Toasts.AddAsync(newToast, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ToastBriefDto>(newToast);
    }
}