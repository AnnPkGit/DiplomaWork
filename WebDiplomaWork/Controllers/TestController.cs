using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess;

namespace WebDiplomaWork.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRepository<TestEntity, string> _repository;

        public TestController(IRepository<TestEntity, string> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            var tests = _repository.GetAll().ToList();
            return tests;
        }
    }
}
