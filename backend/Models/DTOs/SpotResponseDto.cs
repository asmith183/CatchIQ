using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.DTOs;
public record SpotResponseDto(
    int Id,
    int UserId,
    string Name,
    double Latitude,
    double Longitude,
    WaterBodyType WaterBodyType,
    string? Notes
);