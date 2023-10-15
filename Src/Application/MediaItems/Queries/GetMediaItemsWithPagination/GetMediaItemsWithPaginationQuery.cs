using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using Application.MediaItems.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.MediaItems.Queries.GetMediaItemsWithPagination;

[Authorize]
public class GetMediaItemsWithPaginationQuery : IRequest<PaginatedList<MediaItemBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetMediaItemsWithPaginationQueryHandler : IRequestHandler<GetMediaItemsWithPaginationQuery, PaginatedList<MediaItemBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMediaItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MediaItemBriefDto>> Handle(GetMediaItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.MediaItems
            .IgnoreAutoIncludes()
            .OrderBy(mediaItem => mediaItem.Name)
            .ProjectTo<MediaItemBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}