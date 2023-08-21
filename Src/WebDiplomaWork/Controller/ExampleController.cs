using App.Common.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly IExampleService _userService;
        private readonly IMapper _mapper;

        public ExampleController(IExampleService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public object GetById(string id)
        {
            return _userService.GetById(Guid.Parse(id));
        }

        [HttpGet]
        public object GetAll()
        {
            return _userService.GetAll();
        }
    }
}
