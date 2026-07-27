using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CatchIQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthManager authManager) : ControllerBase
{
    private readonly IAuthManager _authManager = authManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authManager.RegisterAsync(registerDto);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authManager.LoginAsync(loginDto);

        if (result == null)
            return Unauthorized();
        else
            return Ok(result);
    }
}