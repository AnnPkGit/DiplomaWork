using App.Service;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Controller
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
        
        
        [HttpPost("register")]
        
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            var result = await _userService.AddUserAsync(userEntity);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }
        
    }
}
