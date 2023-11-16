using Application.Accounts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reactions.Queries.GetReactionsByToast;

public class GetReactionsByToastQuery : IRequest<PaginatedList<AccountBriefDto>>
{
    public int ToastWithContentId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetReactionsByToastQueryHandler : IRequestHandler<GetReactionsByToastQuery, PaginatedList<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReactionsByToastQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AccountBriefDto>> Handle(GetReactionsByToastQuery request, CancellationToken cancellationToken)
    {
        var toastWithContentId = request.ToastWithContentId;
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == toastWithContentId, cancellationToken))
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }
        
        return await _context.Reactions
            .Where(r => r.ToastWithContentId == toastWithContentId)
            .OrderByDescending(r => r.Reacted)
            .Select(r => r.Author)
            .ProjectTo<AccountBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}