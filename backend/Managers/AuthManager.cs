using CatchIQ.API.Engines.Interfaces;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace CatchIQ.API.Managers;
public class AuthManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenEngine tokenEngine) : IAuthManager
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ITokenEngine _tokenEngine = tokenEngine;

    public async Task<(AuthResponseDto? Result, IReadOnlyList<string> Errors)> RegisterAsync(RegisterDto registerDto)
    {
        var userInfo = new ApplicationUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
        };

        var result = await _userManager.CreateAsync(userInfo, registerDto.Password);

        if (result.Succeeded)
            return (_tokenEngine.GenerateToken(userInfo), []);

        // Identity's own descriptions ("Passwords must have at least one digit.",
        // "Email 'x' is already taken.") are user-facing, so pass them through.
        return (null, result.Errors.Select(e => e.Description).ToList());
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

        if (result.Succeeded) 
            return _tokenEngine.GenerateToken(user);

        return null;
    }
}