using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess;

namespace WebDiplomaWork.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<UserEntity, string> _repository;

        public UserController(IRepository<UserEntity, string> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            var users = _repository.GetAll().ToList();
            return users;
        }
    }
}
