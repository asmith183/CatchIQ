namespace CatchIQ.API.Models.DTOs;

public record AuthResponseDto(string Token, DateTime Expiration);