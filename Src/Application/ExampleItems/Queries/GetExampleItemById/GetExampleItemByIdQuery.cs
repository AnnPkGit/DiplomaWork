using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.ExampleItems.Queries.GetExampleItemById;

public record GetExampleItemByIdQuery(int Id) : IRequest<ExampleItem>;

public class GetExampleItemByIdQueryHandler : IRequestHandler<GetExampleItemByIdQuery, ExampleItem>
{
    private readonly IApplicationDbContext _context;

    public GetExampleItemByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExampleItem> Handle(GetExampleItemByIdQuery request, CancellationToken token)
    {
        var entity = await _context.ExampleItems.FindAsync(new object?[] { request.Id }, token);
        if (entity == null)
            throw new NotFoundException(nameof(ExampleItem), request.Id);
        return entity;
    }
}