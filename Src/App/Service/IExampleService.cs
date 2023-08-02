using Domain.Entity;

namespace App.Service;

public interface IExampleService
{
    IQueryable<ExampleItem> GetAll();
    ExampleItem GetById(Guid id);
    ExampleItem Create(ExampleItem exampleItem);
    ExampleItem Update(ExampleItem exampleItem);
    void Delete(Guid id);
}