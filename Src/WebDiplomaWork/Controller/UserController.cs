using App.Common.Interfaces.Services;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;
using WebDiplomaWork.Filters;

namespace WebDiplomaWork.Controller
{
    [ApiExceptionFilter]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
    
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] UserRegistrationDto userDto,
            CancellationToken cancellationToken)
        {
            var userEntity = _mapper.Map<User>(userDto);
            var result = await _userService.CreateUserAsync(userEntity, cancellationToken);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUsersAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPatch("email"), Authorize]
        public async Task<IActionResult> ChangeEmail(
            [FromBody] ChangeEmailDto model,
            CancellationToken cancellationToken)
        {
            // TODO: Implement new email validation
            var result = await _userService.ChangeEmailAsync(model.new_email, cancellationToken);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
        
        [HttpPatch("phone"), Authorize]
        public async Task<IActionResult> ChangePhone(
            [FromBody] ChangePhoneDto model,
            CancellationToken cancellationToken)
        {
            // TODO: Implement new phone number validation
            var result = await _userService.ChangePhoneAsync(model.new_phone, cancellationToken);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
        
        [HttpPatch("password"), Authorize]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto model,
            CancellationToken cancellationToken)
        {
            // TODO: Implement new password validation
            var result = await _userService.ChangePasswordAsync(model.new_password, cancellationToken);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
    }

    public record ChangePhoneDto(string new_phone);
    public record ChangeEmailDto(string new_email);
    public record ChangePasswordDto(string new_password);
}