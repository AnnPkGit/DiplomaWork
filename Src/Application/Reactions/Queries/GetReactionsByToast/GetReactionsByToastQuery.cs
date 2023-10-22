using Application.Accounts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reactions.Queries.GetReactionsByToast;

public class GetReactionsByToastQuery : IRequest<PaginatedList<AccountBriefDto>>
{
    public int ToastId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetReactionsByToastQueryHandler : IRequestHandler<GetReactionsByToastQuery, PaginatedList<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReactionsByToastQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AccountBriefDto>> Handle(GetReactionsByToastQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Toasts.AnyAsync(t => t.Id == request.ToastId, cancellationToken))
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var reactions = _context.Reactions
            .Where(r => r.ToastId == request.ToastId)
            .OrderByDescending(r => r.Reacted)
            .Include(r => r.Author)
            .Select(r => r.Author);

        return await reactions
            .ProjectTo<AccountBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}