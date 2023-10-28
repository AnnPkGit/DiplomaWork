using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Quotes.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quotes.Queries.GetQuotesByToast;

public class GetQuotesByToastQuery : IRequest<PaginatedList<QuoteDto>>
{
    public int ToastWithContentId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetQuotesByToastQueryHandler : IRequestHandler<GetQuotesByToastQuery, PaginatedList<QuoteDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuotesByToastQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QuoteDto>> Handle(GetQuotesByToastQuery request, CancellationToken cancellationToken)
    {
        var toastWithContentId = request.ToastWithContentId;
        
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == toastWithContentId, cancellationToken))
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }
        
        return await _context.Quotes
            .Where(q => q.QuotedToastId == toastWithContentId)
            .OrderByDescending(q => q.Created)
            .Include(q => q.Replies)
            .Include(q => q.Reactions)
            .Include(q => q.ReToasts)
            .Include(q => q.Quotes)
            .ProjectTo<QuoteDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

