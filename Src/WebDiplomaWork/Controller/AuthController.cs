using App.Common.Interfaces.Services;
using App.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;
using WebDiplomaWork.Filters;

namespace WebDiplomaWork.Controller
{
    [ApiExceptionFilter]
    [Route("api/v1/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(
            [FromBody] LoginRequestDto loginRequestDto,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<LoginRequest>(loginRequestDto);
            var tokenResult = await _userService.LoginUserAsync(request, cancellationToken);
            
            if (!tokenResult.IsSuccessful)
            {
                return BadRequest(tokenResult.ErrorMessage);
            }
            
            return Ok(tokenResult.Value);
        }
    }
}
