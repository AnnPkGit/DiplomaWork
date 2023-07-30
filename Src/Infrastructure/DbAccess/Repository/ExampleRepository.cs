using App.Repository;
using AutoMapper;
using Domain.Entity;
using Infrastructure.DbAccess.EfDbContext;

namespace Infrastructure.DbAccess.Repository;

public class ExampleRepository : IExampleRepository
{
    private readonly ExampleContext _context;
    private readonly IMapper _mapper;
    
    public ExampleRepository(ExampleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IQueryable<ExampleItem> GetAll()
    {
        return _mapper.ProjectTo<ExampleItem>(_context.Example);
    }

    public ExampleItem? FindById(Guid id)
    {
        var result = _context.Example.Find(id.ToString());
        return _mapper.Map<ExampleItem>(result);
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