using Domain.Entity;

namespace App.Repository;

public interface IExampleRepository
{
    IQueryable<ExampleItem> GetAll();
    ExampleItem? FindById(Guid id);
    ExampleItem Create(ExampleItem exampleItem);
    ExampleItem Update(ExampleItem exampleItem);
    void Delete(Guid id);
}