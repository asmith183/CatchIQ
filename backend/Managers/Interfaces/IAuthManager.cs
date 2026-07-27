using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Managers.Interfaces;
public interface IAuthManager
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
}
