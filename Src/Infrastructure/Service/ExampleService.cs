using App.Repository;
using App.Service;
using Domain.Entity;

namespace Infrastructure.Service;

public class ExampleService : IExampleService
{
    private readonly IExampleRepository _repository;
    public ExampleService(IExampleRepository repository)
    {
        _repository = repository;
    }

    public IQueryable<ExampleItem> GetAll()
    {
        return _repository.GetAll();
    }

    public ExampleItem GetById(Guid id)
    {
        return _repository.GetAll().FirstOrDefault(test => test.Id == id);
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