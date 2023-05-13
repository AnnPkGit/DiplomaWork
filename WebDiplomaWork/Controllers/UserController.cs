using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.App;
using WebDiplomaWork.DB.DTOs;
using WebDiplomaWork.Domain.Entities;

namespace WebDiplomaWork.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _userService.AddUserAsync(userEntity);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
    }
}
