using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.ViewModels.DTO;
using SmartWealth.AuthService.Utilities.Exceptions;
using SmartWealth.AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SmartWealth.AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService service) : Controller
{
    private readonly IAuthService _service = service;
    private readonly Response _response = new();

    [Authorize]
    [HttpGet("exist/{id:guid}")]
    public async Task<IActionResult> IsUserExist(Guid id)
    {
        try
        {
            _response.Data = await _service.IsUserExistAsync(id);
            _response.Message = string.Empty;

            return Ok(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccess = false;
            _response.Message = exception.Message;

            return BadRequest(_response);
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserRegistrationViewModel userRegistration)
    {
        try
        {
            _response.Data = await _service.RegisterAsync(userRegistration);
            _response.Message = "User is successfully registered";

            return Ok(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccess = false;
            _response.Message = exception.Message;

            return BadRequest(_response);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginViewModel userLogin)
    {
        try
        {
            _response.Data = await _service.LoginAsync(userLogin);
            _response.Message = "User is successfully logged in";

            return Ok(_response);
        }
        catch (NotFoundException notFoundException)
        {
            _response.IsSuccess = false;
            _response.Message = notFoundException.Message;

            return BadRequest(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccess = false;
            _response.Message = exception.Message;

            return BadRequest(_response);
        }
    }
}