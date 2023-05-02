using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.Infrastructure.DbAccess;

namespace WebDiplomaWork.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _userContext;

        public UserController(DataContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            var users = _userContext.Users.ToList();
            _userContext.Dispose();
            return users;
        }
    }
}
