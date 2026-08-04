using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.DTOs;
public record UpdateSpotDto(
    string Name,
    double Latitude,
    double Longitude,
    WaterBodyType WaterBodyType,
    string? Notes
);
