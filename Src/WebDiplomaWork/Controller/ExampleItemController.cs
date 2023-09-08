using Application.ExampleItems.Commands.DeleteExampleItem;
using Application.ExampleItems.Queries.GetAllExampleItems;
using Application.ExampleItems.Queries.GetExampleItemById;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;
public class ExampleItemController : ApiV1ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ExampleItem> GetById(int id)
    {
        return await Mediator.Send(new GetExampleItemByIdQuery(id));
    }

    [HttpGet]
    public async Task<IEnumerable<ExampleItem>> GetAll()
    {
        return await Mediator.Send(new GetAllExampleItemsQuery());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteExampleItemCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}