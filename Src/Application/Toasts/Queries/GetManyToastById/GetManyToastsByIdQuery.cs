using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Queries.GetManyToastById;

public record GetManyToastsByIdQuery(params int[] ToastIds) : IRequest<IEnumerable<ToastBriefDto>>;

public class GetManyToastByIdQueryHandler : IRequestHandler<GetManyToastsByIdQuery, IEnumerable<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GetManyToastByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<IEnumerable<ToastBriefDto>> Handle(GetManyToastsByIdQuery request, CancellationToken cancellationToken)
    {
        var myAccountId = _userService.Id;
        var idsCount = request.ToastIds.Length;
        var toastDtoArray = new Toast[idsCount];

        var contextToasts = _context.Toasts
            .Include(t => t.Replies).ThenInclude(r => r.Replies)
            .Include(t => t.Replies).ThenInclude(r => r.Reactions)
            .Include(t => t.Replies).ThenInclude(r => r.Quotes)
            .Include(t => t.Replies).ThenInclude(r => r.ReToasts)
            .Include(t => t.Reactions)
            .Include(t => t.Quotes)
            .Include(t => t.ReToasts)
            .Include(t => t.Reply).ThenInclude(r => r!.Replies)
            .Include(t => t.Reply).ThenInclude(r => r!.Reactions)
            .Include(t => t.Reply).ThenInclude(r => r!.Quotes)
            .Include(t => t.Reply).ThenInclude(r => r!.ReToasts)
            .Include(t => t.Quote)
            .Include(t => t.ReToast).ThenInclude(rt => rt!.Replies)
            .Include(t => t.ReToast).ThenInclude(rt => rt!.Reactions)
            .Include(t => t.ReToast).ThenInclude(rt => rt!.Quotes)
            .Include(t => t.ReToast).ThenInclude(rt => rt!.ReToasts);
        
        for (var i = 0; i < idsCount; i++)
        {
            var item = await contextToasts
                .SingleOrDefaultAsync(t => t.Id == request.ToastIds[i], cancellationToken);

            toastDtoArray[i] = item ?? throw new NotFoundException(nameof(Toast), i);
        }
        
        var selectedToasts = ToastSelectModel.SelectToasts(toastDtoArray, myAccountId)
            .Select(tsm => _mapper.Map<ToastBriefDto>(tsm));
        
        return selectedToasts;
    }
}