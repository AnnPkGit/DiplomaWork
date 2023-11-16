using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Replies.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Replies.Queries.GetReplyById;

public record GetReplyByIdQuery(int ReplyId) : IRequest<ReplyDto>;

public class GetReplyByIdQueryHandler : IRequestHandler<GetReplyByIdQuery, ReplyDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReplyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReplyDto> Handle(GetReplyByIdQuery request, CancellationToken cancellationToken)
    {
        var replyId = request.ReplyId;
        if (!await _context.Replies.AnyAsync(r => r.Id == replyId, cancellationToken))
        {
            throw new NotFoundException(nameof(Reply), request.ReplyId);
        }

        var reply = await _context.Replies
            .Include(r => r.ReplyToToast).ThenInclude(t => t.Replies)
            .Include(r => r.ReplyToToast).ThenInclude(t => t.Reactions)
            .Include(r => r.ReplyToToast).ThenInclude(t => t.Quotes)
            .Include(r => r.ReplyToToast).ThenInclude(t => t.ReToasts)
            .SingleOrDefaultAsync(r => r.Id == replyId, cancellationToken);
        
        return _mapper.Map<ReplyDto>(reply);
    }
}