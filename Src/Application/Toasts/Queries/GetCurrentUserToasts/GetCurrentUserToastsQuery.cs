using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Queries.GetCurrentUserToasts;

[Authorize]
public class GetCurrentUserToastsQuery : IRequest<PaginatedList<ToastBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCurrentUserToastsQueryHandler : IRequestHandler<GetCurrentUserToastsQuery, PaginatedList<ToastBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;

    public GetCurrentUserToastsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IMapper mapper)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ToastBriefDto>> Handle(GetCurrentUserToastsQuery request, CancellationToken token)
    {
        var userId = _userService.Id;
        var account = await _context.Accounts
            .AsNoTracking()
            .Include(a => a.ReToasts)!
            .ThenInclude(a => a.Replies)!
            .ThenInclude(a => a.ReToasters)!
            .ThenInclude(a => a.Reactions)
            .SingleOrDefaultAsync(a => a.Id == userId, token);
        
        if (account == null)
        {
            throw new NotFoundException(nameof(Account), userId);
        }

        var toasts = await _context.Toasts
            .AsNoTracking()
            .Include(t => t.Replies)!
            .ThenInclude(r => r.Author)
            .Where(t => t.AuthorId == userId && t.Type != "reply").ToArrayAsync(token);
        var reToasts = account.ReToasts?
            .Join(_context.ReToasts, t => t.Id, rt => rt.ToastId,
                (toast, reToast) =>
                {
                    toast.Created = reToast.Created;
                    return toast;
                })
            .ToArray();

        var briefToasts = _mapper.Map<Toast[], ToastBriefDto[]>(toasts);
        var briefReToasts = _mapper.Map<Toast[]?, ToastBriefDto[]?>(reToasts);
        
        var allToasts = briefReToasts == null 
            ? briefToasts
            : briefToasts
                .Concat(briefReToasts
                    .Select(rt =>
                    {
                        rt.IsReToast = true;
                        return rt;
                    }))
                .ToArray();
            
        return allToasts
            .OrderByDescending(t => t.Created)
            .PaginatedArray(request.PageNumber, request.PageSize);
    }
}