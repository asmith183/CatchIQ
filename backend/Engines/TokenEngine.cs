using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CatchIQ.API.Engines.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace CatchIQ.API.Engines;
public class TokenEngine : ITokenEngine
{
    private readonly IConfiguration _configuration;

    public TokenEngine(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResponseDto GenerateToken(ApplicationUser user)
    {
        var secret = _configuration["Jwt:Secret"]!;
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        var expiration = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponseDto(tokenString, expiration);
    }
}