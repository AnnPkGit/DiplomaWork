using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Queries.GetRepliesByToast;

public class GetRepliesByToastQuery : IRequest<PaginatedList<ReplyBriefDto>>
{
    public int ToastId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetRepliesByToastQueryHandler : IRequestHandler<GetRepliesByToastQuery, PaginatedList<ReplyBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;
    
    public GetRepliesByToastQueryHandler(
        IMapper mapper,
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _mapper = mapper;
        _context = context;
        _userService = userService;
    }

    public async Task<PaginatedList<ReplyBriefDto>> Handle(GetRepliesByToastQuery request, CancellationToken token)
    {
        var myAccountId = _userService.Id;
        if (!await _context.Toasts.AnyAsync(t => t.Id == request.ToastId, token))
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        var toastReplies = await _context.Toasts
            .Where(t => t.ReplyId == request.ToastId)
            .OrderByDescending(t => t.Created)
            .GetPaginatedSource(request.PageNumber, request.PageSize, out var totalCount)
            .Include(t => t.Replies)
            .Include(t => t.Reactions)
            .Include(t => t.Quotes)
            .Include(t => t.ReToasts)
            .ToArrayAsync(token);

        var selectedReplies = ToastSelectModel.SelectQuotes(toastReplies, myAccountId)
            .Select(tsm => _mapper.Map<ReplyBriefDto>(tsm));

        return selectedReplies
            .ToPaginatedArray(request.PageNumber, request.PageSize, totalCount);
    }
}