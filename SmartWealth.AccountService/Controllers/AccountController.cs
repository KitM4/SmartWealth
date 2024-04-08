using SmartWealth.AccountService.ViewModels;
using SmartWealth.AccountService.Services.Interfaces;
using SmartWealth.AccountService.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SmartWealth.AccountService.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountService service) : Controller
{
    private readonly IAccountService _service = service;

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            return Ok(await _service.GetAccountsAsync());
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetAccountsByUser(Guid userId)
    {
        try
        {
            return Ok(await _service.GetAccountsByUserAsync(userId));
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        try
        {
            return Ok(await _service.GetAccountAsync(id));
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountViewModel createdAccount)
    {
        try
        {
            await _service.CreateAccountAsync(createdAccount);
            return Created();
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpPut("edit/{id:guid}")]
    public async Task<IActionResult> EditAccount(Guid id, [FromBody] AccountViewModel editedAccount)
    {
        try
        {
            await _service.EditAccountAsync(id, editedAccount);
            return Created();
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        try
        {
            await _service.DeleteAccountAsync(id);
            return NoContent();
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpGet("defaults/{userId:guid}")]
    public async Task<IActionResult> GenerateDefaultAccounts(Guid userId)
    {
        try
        {
            return Ok(await _service.GenerateDefaultAccountsAsync(userId));
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}