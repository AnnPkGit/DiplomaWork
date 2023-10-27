using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetToastsMarkedByAccount;

public class GetToastsMarkedByAccountQuery : IRequest<PaginatedList<object>>
{
    public int AccountId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetToastsMarkedByAccountQueryHandler : IRequestHandler<GetToastsMarkedByAccountQuery, PaginatedList<object>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetToastsMarkedByAccountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<object>> Handle(GetToastsMarkedByAccountQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts.AnyAsync(a => a.Id == request.AccountId, cancellationToken))
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }
        
        var destinationType = typeof(BaseToastWithContentDto);
        
        return await _context.Reactions
            .Where(r => r.AuthorId == request.AccountId)
            .OrderByDescending(r => r.Reacted)
            .Include(r => r.ToastWithContent).ThenInclude(t => t.Replies)
            .Include(r => r.ToastWithContent).ThenInclude(t => t.Reactions)
            .Include(r => r.ToastWithContent).ThenInclude(t => t.ReToasts)
            .Include(r => r.ToastWithContent).ThenInclude(t => t.Quotes)
            .Select(r => _mapper.Map(r.ToastWithContent, r.ToastWithContent.GetType(), destinationType))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}