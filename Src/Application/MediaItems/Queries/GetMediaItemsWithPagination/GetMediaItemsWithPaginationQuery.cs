using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.MediaItems.Queries.GetMediaItemsWithPagination;

[Authorize(Roles = "SuperAdministrator")]
public class GetMediaItemsWithPaginationQuery : IRequest<PaginatedList<MediaItem>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetMediaItemsWithPaginationQueryHandler : IRequestHandler<GetMediaItemsWithPaginationQuery, PaginatedList<MediaItem>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMediaItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MediaItem>> Handle(GetMediaItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.MediaItems
            .IgnoreAutoIncludes()
            .OrderBy(mediaItem => mediaItem.Name)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}