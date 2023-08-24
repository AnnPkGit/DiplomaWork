using App.Common.Interfaces.Services;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;

namespace WebDiplomaWork.Controller;

[Route("api/v1/[controller]")]
[ApiController]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountController(
        IAccountService accountService,
        IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Post(CreateAccountDto accountDto, CancellationToken cancellationToken)
    {
        var newAccount = _mapper.Map<Account>(accountDto);
        var result = await _accountService.CreateAccountAsync(newAccount ,cancellationToken);
        if (!result.IsSuccessful)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _accountService.GetAllAccounts(cancellationToken);
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }
}