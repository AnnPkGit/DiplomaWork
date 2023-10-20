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

namespace Application.Toasts.Queries.GetReToastsByToast;

public class GetReToastsByToastQuery : IRequest<PaginatedList<AccountBriefDto>>
{
    public int ToastId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetReToastsByToastQueryHandler : IRequestHandler<GetReToastsByToastQuery, PaginatedList<AccountBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetReToastsByToastQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AccountBriefDto>> Handle(GetReToastsByToastQuery request, CancellationToken token)
    {
        if (!await _context.Toasts.AnyAsync(t => t.Id == request.ToastId, token))
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var toastReToasts = _context.Toasts
            .Where(t => t.ReToastId == request.ToastId)
            .OrderByDescending(t => t.Created)
            .Select(t => t.Author);

        return await toastReToasts
            .ProjectTo<AccountBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}