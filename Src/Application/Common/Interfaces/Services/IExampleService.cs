using Domain.Entity;

namespace Application.Common.Interfaces.Services;

public interface IExampleService
{
    IQueryable<ExampleItem> GetAll();
    ExampleItem GetById(int id);
    ExampleItem Create(ExampleItem exampleItem);
    ExampleItem Update(ExampleItem exampleItem);
    void Delete(int id);
}