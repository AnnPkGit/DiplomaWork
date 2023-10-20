using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Queries.GetQuotesByToast;

public class GetQuotesByToastQuery : IRequest<PaginatedList<ToastBriefDto>>
{
    public int ToastId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetQuotesByToastQueryHandler : IRequestHandler<GetQuotesByToastQuery, PaginatedList<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;

    public GetQuotesByToastQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IMapper mapper)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ToastBriefDto>> Handle(GetQuotesByToastQuery request, CancellationToken token)
    {
        var myAccountId = _userService.Id;
        if (!await _context.Toasts.AnyAsync(t => t.Id == request.ToastId, token))
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var toastQuotes = await _context.Toasts
            .Where(t => t.QuoteId == request.ToastId)
            .OrderByDescending(t => t.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Include(t => t.Replies)
            .Include(t => t.Reactions)
            .Include(t => t.Quotes)
            .Include(t => t.ReToasts)
            .Include(t => t.Quote)
            .ToArrayAsync(token);

        var selectedQuotes = ToastSelectModel.SelectReplies(toastQuotes, myAccountId)
            .Select(tsm => _mapper.Map<ToastBriefDto>(tsm));
        
        return selectedQuotes
            .ToPaginatedArray(request.PageNumber, request.PageSize, totalCount);
    }
}