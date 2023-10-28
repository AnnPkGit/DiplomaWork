using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Replies.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Replies.Queries.GetAllReplies;

public class GetAllRepliesQuery : IRequest<PaginatedList<ReplyBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAllRepliesQueryHandler : IRequestHandler<GetAllRepliesQuery, PaginatedList<ReplyBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllRepliesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReplyBriefDto>> Handle(GetAllRepliesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Replies
            .OrderByDescending(r => r.Created)
            .ProjectTo<ReplyBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}