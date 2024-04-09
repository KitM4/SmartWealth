using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.Services.Interfaces;
using SmartWealth.AuthService.Utilities.Exceptions;

namespace SmartWealth.AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService service) : Controller
{
    private readonly IAuthService _service = service;

    [Authorize]
    [HttpGet("exist/{id:guid}")]
    public async Task<IActionResult> IsUserExist(Guid id)
    {
        try
        {
            return Ok(await _service.IsUserExistAsync(id));
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserViewModel userViewModel)
    {
        try
        {
            return Ok(await _service.RegisterAsync(userViewModel));
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserViewModel userViewModel)
    {
        try
        {
            return Ok(await _service.LoginAsync(userViewModel));
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
    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromForm] UserViewModel userViewModel)
    {
        try
        {
            return Ok(await _service.UpdateUserAsync(userViewModel));
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
}