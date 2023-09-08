using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.ExampleItems.Commands.DeleteExampleItem;

public record DeleteExampleItemCommand(int Id) : IRequest;

public class DeleteExampleItemCommandHandler : IRequestHandler<DeleteExampleItemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteExampleItemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteExampleItemCommand request, CancellationToken token)
    {
        var item = await _dbContext.ExampleItems.FindAsync(new object?[] { request.Id }, token);
        if (item == null)
            throw new NotFoundException(nameof(ExampleItem), request.Id);
        item.DeactivationDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(token);
    }
}