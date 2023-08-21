using App.Common.Exceptions;
using App.Common.Interfaces;
using App.Common.Interfaces.Services;
using Domain.Entity;

namespace App.Services;

public class ExampleService : IExampleService
{
    private readonly IApplicationDbContext _dbContext;

    public ExampleService(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<ExampleItem> GetAll()
    {
        return _dbContext.ExampleItems;
    }

    public ExampleItem GetById(Guid id)
    {
        var result = _dbContext.ExampleItems.SingleOrDefault(test => test.Id == id);
        if (result == null)
        {
            throw new NotFoundException("ExampleItem", id);
        }
        return result;
    }

    public ExampleItem Create(ExampleItem exampleItem)
    {
        throw new NotImplementedException();
    }

    public ExampleItem Update(ExampleItem exampleItem)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}