using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces.Services;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.DTO;
using WebDiplomaWork.Filters;

namespace WebDiplomaWork.Controller;

[ApiExceptionFilter]
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
    public async Task<IActionResult> Post(
        CreateAccountDto accountDto,
        CancellationToken cancellationToken)
    {
        var newAccount = _mapper.Map<Account>(accountDto);
        var result = await _accountService.CreateAccountAsync(newAccount, cancellationToken);
        if (!result.IsSuccessful)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var res = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(res);
    }
    
    [HttpGet("{login}")]
    public async Task<IActionResult> GetByLogin(string login, CancellationToken cancellationToken)
    {
        var res = await _accountService.GetAccountByLoginAsync(login, cancellationToken);
        return Ok(res);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _accountService.GetAllAccounts(cancellationToken);
        if (!result.Any())
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _accountService.DeleteAccountAsync(id, cancellationToken);
        if (!result.IsSuccessful)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok("Account deleted");
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> Put(
        [FromBody] FullyUpdateAccountDto accountDto,
        CancellationToken cancellationToken)
    {
        var id = accountDto.Id;
        var model = _mapper.Map<UpdateAccountModel>(accountDto);
        var result = await _accountService
            .FullyUpdateAccountAsync(id, model, cancellationToken);
        
        if (!result.IsSuccessful)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return Ok("Account changed");
    }

    [HttpPatch, Authorize]
    public async Task<IActionResult> Patch(
        [FromBody] UpdateAccountDto accountDto,
        CancellationToken cancellationToken)
    {
        var id = accountDto.Id;
        var model = _mapper.Map<UpdateAccountModel>(accountDto);
        var result = await _accountService
            .UpdateAccountAsync(id, model, cancellationToken);
        
        return Ok(result);
    }
}

public record FullyUpdateAccountDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Login { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}

public record UpdateAccountDto
{
    [Required]
    public int Id { get; set; }
    public string? Login { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}