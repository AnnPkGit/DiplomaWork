using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Replies.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        
        return await _context.Replies
            .Where(r => r.ReplyToToastId == request.ToastWithContentId)
            .OrderByDescending(r => r.Created)
            .ProjectTo<ReplyBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}