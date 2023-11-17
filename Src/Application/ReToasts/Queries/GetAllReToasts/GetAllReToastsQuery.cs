using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.ReToasts.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.ReToasts.Queries.GetAllReToasts;

public class GetAllReToastsQuery : IRequest<PaginatedList<ReToastDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAllReToastsQueryHandler : IRequestHandler<GetAllReToastsQuery, PaginatedList<ReToastDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReToastsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReToastDto>> Handle(GetAllReToastsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReToasts
            .OrderByDescending(rt => rt.Created)
            .ProjectTo<ReToastDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}