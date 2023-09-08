using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ExampleItems.Queries.GetAllExampleItems;

public record GetAllExampleItemsQuery : IRequest<IEnumerable<ExampleItem>>;

public class GetAllExampleItemsQueryHandler : IRequestHandler<GetAllExampleItemsQuery, IEnumerable<ExampleItem>>
{
    private readonly IApplicationDbContext _context;

    public GetAllExampleItemsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExampleItem>> Handle(GetAllExampleItemsQuery request, CancellationToken token)
    {
        return await _context.ExampleItems.ToListAsync(token);
    }
}