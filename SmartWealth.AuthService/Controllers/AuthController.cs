using Microsoft.AspNetCore.Mvc;
using SmartWealth.AuthService.Services;
using SmartWealth.AuthService.Utilities.Exceptions;
using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService service) : Controller
{
    private readonly IAuthService _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationViewModel userRegistration)
    {
        try
        {
            return Ok(await _service.RegisterAsync(userRegistration));
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginViewModel userLogin)
    {
        try
        {
            return Ok(await _service.LoginAsync(userLogin));
        }
        catch (NotFoundException notFoundException)
        {
            return BadRequest(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}