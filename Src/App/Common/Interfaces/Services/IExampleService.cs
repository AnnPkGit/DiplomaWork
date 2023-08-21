using Domain.Entity;

namespace App.Common.Interfaces.Services;

public interface IExampleService
{
    IQueryable<ExampleItem> GetAll();
    ExampleItem GetById(Guid id);
    ExampleItem Create(ExampleItem exampleItem);
    ExampleItem Update(ExampleItem exampleItem);
    void Delete(Guid id);
}