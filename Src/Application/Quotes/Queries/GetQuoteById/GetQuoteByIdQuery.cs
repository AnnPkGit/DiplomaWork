using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quotes.Queries.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quotes.Queries.GetQuoteById;

public record GetQuoteByIdQuery(int QuoteId) : IRequest<QuoteDto>;

public class GetQuoteByIdQueryHandler : IRequestHandler<GetQuoteByIdQuery, QuoteDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuoteByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuoteDto> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
    {
        var quoteId = request.QuoteId;
        if (!await _context.Quotes.AnyAsync(q => q.Id == quoteId, cancellationToken))
        {
            throw new NotFoundException(nameof(Quote), quoteId);
        }
        
        var quote = await _context.Quotes
            .Include(q => q.Replies)
            .Include(q => q.Reactions)
            .Include(q => q.ReToasts)
            .Include(q => q.Quotes)
            .Include(q => q.QuotedToast)
            .SingleOrDefaultAsync(q => q.Id == quoteId, cancellationToken);
        
        return _mapper.Map<QuoteDto>(quote);
    }
}