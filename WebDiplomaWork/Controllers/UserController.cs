using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;

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
