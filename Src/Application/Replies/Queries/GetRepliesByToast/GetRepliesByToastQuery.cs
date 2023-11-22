using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Replies.Queries.Models;
using AutoMapper;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Replies.Queries.GetRepliesByToast;

public class GetRepliesByToastQuery : IRequest<PaginatedList<ReplyBriefDto>>
{
    public int ToastWithContentId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetRepliesByToastQueryHandler : IRequestHandler<GetRepliesByToastQuery, PaginatedList<ReplyBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRepliesByToastQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReplyBriefDto>> Handle(GetRepliesByToastQuery request, CancellationToken cancellationToken)
    {
        var toastWithContentId = request.ToastWithContentId;
        
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == toastWithContentId, cancellationToken))
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }
        
        var toastReplies = await _context.Replies
            .Where(r => r.ReplyToToastId == request.ToastWithContentId)
            .IgnoreAutoIncludes()
            .Include(r => r.Author)
            .Include(r => r.Replies)
            .Include(r => r.Reactions)
            .Include(r => r.Quotes)
            .Include(r => r.ReToasts)
            .Include(r => r.MediaItems)
            .OrderByDescending(r => r.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .ToArrayAsync(cancellationToken);

        var toastRepliesDtos = toastReplies.Select(reply => _mapper.Map<ReplyBriefDto>(reply)).ToArray();
        
        return new PaginatedList<ReplyBriefDto>(toastRepliesDtos, totalCount, request.PageNumber, request.PageSize);
    }
}