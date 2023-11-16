using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Toasts.Queries.GetAllToasts;

public class GetAllToastsQuery : IRequest<PaginatedList<ToastBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAllToastsQueryHandler : IRequestHandler<GetAllToastsQuery, PaginatedList<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllToastsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ToastBriefDto>> Handle(GetAllToastsQuery request, CancellationToken token)
    {
        var toasts = _context.Toasts.ToList();
        return await _context.Toasts
            .OrderByDescending(t => t.Created)
            .ProjectTo<ToastBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}