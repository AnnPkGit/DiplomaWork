using Application.Common.Interfaces;
using Application.MediaItems.Queries.Models;
using AutoMapper;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.MediaItems.Queries.GetAllMediaItems;

public record GetAllMediaItemsQuery : IRequest<IEnumerable<BaseMediaItemDto>>;

public class GetAllMediaItemsQueryHandler : IRequestHandler<GetAllMediaItemsQuery, IEnumerable<BaseMediaItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllMediaItemsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BaseMediaItemDto>> Handle(GetAllMediaItemsQuery request, CancellationToken cancellationToken)
    {
        var toastMediaItems = await _context.ToastMediaItems.ToArrayAsync(cancellationToken);
        var avatarMediaItems = await _context.AvatarMediaItems.ToArrayAsync(cancellationToken);

        return toastMediaItems
            .Concat(avatarMediaItems as BaseMediaItem[])
            .Select(item => _mapper.Map<BaseMediaItemDto>(item));
    }
}