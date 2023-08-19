using App.Service;
using App.Users.Login;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TokenController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            var request = _mapper.Map<LoginRequest>(loginRequestDto);
            var tokenResult = await _userService.LoginUserAsync(request);
            
            if (!tokenResult.IsSuccessful)
            {
                return BadRequest(tokenResult.ErrorMessage);
            }
            
            return Ok(tokenResult.Value);
        }
    }
}
