using App.Service;
using App.Users.Login;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Controller
{
    [Route("api/v1/[controller]")]
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
        public async Task<IActionResult> Create([FromBody] UserRegistrationDto userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            var result = await _userService.CreateUserAsync(userEntity);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }
    }
}
