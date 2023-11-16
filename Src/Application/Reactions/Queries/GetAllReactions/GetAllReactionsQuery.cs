using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Reactions.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Reactions.Queries.GetAllReactions;

public class GetAllReactionsQuery : IRequest<PaginatedList<ReactionBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAllReactionsQueryHandler : IRequestHandler<GetAllReactionsQuery, PaginatedList<ReactionBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReactionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReactionBriefDto>> Handle(GetAllReactionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reactions
            .OrderByDescending(r => r.Reacted)
            .ProjectTo<ReactionBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}