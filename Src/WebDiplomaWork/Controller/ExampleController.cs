using App.Common.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.Filters;

namespace WebDiplomaWork.Controller
{
    [ApiExceptionFilter]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly IExampleService _exampleService;
        private readonly IMapper _mapper;

        public ExampleController(IExampleService userService, IMapper mapper)
        {
            _exampleService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public object GetById(int id)
        {
            return _exampleService.GetById(id);
        }

        [HttpGet]
        public object GetAll()
        {
            return _exampleService.GetAll();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            _exampleService.Delete(id);
            return Ok("Item deleted");
        }
    }
}
