using Application.BaseToasts.Queries.Models;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quotes.Queries.Models;
using Application.Replies.Queries.Models;
using Application.Toasts.Queries.Models;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Queries.GetToastWithContentById;

public record GetToastWithContentByIdQuery(int ToastWithContentId) : IRequest<BaseToastWithContentDto>;

public class GetToastWithContentByIdQueryHandler : IRequestHandler<GetToastWithContentByIdQuery, BaseToastWithContentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetToastWithContentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseToastWithContentDto> Handle(GetToastWithContentByIdQuery request, CancellationToken cancellationToken)
    {
        var toastWithContentId = request.ToastWithContentId;
        var type = await _context.BaseToastsWithContent
            .Where(t => t.Id == toastWithContentId)
            .Select(t => t.Type).SingleOrDefaultAsync(cancellationToken);
        
        if (type == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }
        
        switch (type)
        {
            case nameof(Toast): 
                var toast = await _context.Toasts
                    .Include(t => t.Replies)
                    .Include(t => t.Reactions)
                    .Include(t => t.ReToasts)
                    .Include(t => t.Quotes)
                    .SingleAsync(t => t.Id == toastWithContentId, cancellationToken);
                return _mapper.Map<ToastBriefDto>(toast);
            case nameof(Reply):
                var reply = await _context.Replies
                    .Include(r => r.Replies)
                    .Include(r => r.Reactions)
                    .Include(r => r.ReToasts)
                    .Include(r => r.Quotes)
                    .Include(r => r.ReplyToToast).ThenInclude(t => t.Replies)
                    .Include(r => r.ReplyToToast).ThenInclude(t => t.Reactions)
                    .Include(r => r.ReplyToToast).ThenInclude(t => t.Quotes)
                    .Include(r => r.ReplyToToast).ThenInclude(t => t.ReToasts)
                    .SingleAsync(r => r.Id == toastWithContentId, cancellationToken);
                return _mapper.Map<ReplyDto>(reply);
            case nameof(Quote):
                var quote = await _context.Quotes
                    .Include(q => q.Replies)
                    .Include(q => q.Reactions)
                    .Include(q => q.ReToasts)
                    .Include(q => q.Quotes)
                    .Include(q => q.QuotedToast)
                    .SingleAsync(q => q.Id == toastWithContentId, cancellationToken);
                return _mapper.Map<QuoteDto>(quote);
            default:
                return null!;
        }
    }
}