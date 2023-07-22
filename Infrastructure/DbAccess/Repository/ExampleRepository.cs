using App.Repository;
using Domain.Entity;
using Infrastructure.DbAccess.EfDbContext;

namespace Infrastructure.DbAccess.Repository;

public class ExampleRepository : IExampleRepository
{
    private readonly ExampleContext _context;
    
    public ExampleRepository(ExampleContext context)
    {
        _context = context;
    }

    public IQueryable<ExampleItem> GetAll()
    {
        return _context.Example;
    }

    public ExampleItem? FindById(Guid id)
    {
        var result = _context.Example.Find(id.ToString());
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