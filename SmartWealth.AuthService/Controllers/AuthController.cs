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
    public async Task<IActionResult> Register([FromForm] UserViewModel userViewModel)
    {
        try
        {
            _response.Data = await _service.RegisterAsync(userViewModel);
            _response.Message = "User successfully registered";

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
    public async Task<IActionResult> Login([FromBody] UserViewModel userViewModel)
    {
        try
        {
            _response.Data = await _service.LoginAsync(userViewModel);
            _response.Message = "User successfully logged in";

            return Ok(_response);
        }
        catch (NotFoundException notFoundException)
        {
            _response.IsSuccess = false;
            _response.Message = notFoundException.Message;

            return NotFound(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccess = false;
            _response.Message = exception.Message;

            return BadRequest(_response);
        }
    }

    [Authorize]
    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromForm] UserViewModel userViewModel)
    {
        try
        {
            _response.Data = await _service.UpdateUserAsync(userViewModel);
            _response.Message = "User successfully updated";

            return Ok(_response);
        }
        catch (NotFoundException notFoundException)
        {
            _response.IsSuccess = false;
            _response.Message = notFoundException.Message;

            return NotFound(_response);
        }
        catch (Exception exception)
        {
            _response.IsSuccess = false;
            _response.Message = exception.Message;

            return BadRequest(_response);
        }
    }
}