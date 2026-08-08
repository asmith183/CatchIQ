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
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var (result, errors) = await _authManager.RegisterAsync(registerDto);

        if (result == null)
            return BadRequest(new { errors });

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var result = await _authManager.LoginAsync(loginDto);

        if (result == null)
            return Unauthorized();
        else
            return Ok(result);
    }
}