using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.DTOs;
public record CatchResponseDto(
    int Id,
    int UserId,
    int SpeciesId,
    int? SpotId,
    int? BaitId,
    double Latitude,
    double Longitude,
    DateTimeOffset CaughtAt,
    double? WeightLbs,
    WeightMethod? WeightMethod,
    double? LengthInches,
    double? AirTempF,
    double? WindSpeedMph,
    WindDirection? WindDirection,
    double? PressureMb,
    SkyCondition? SkyCondition,
    double? WaterTempF,
    string? Notes,
    string? PhotoUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
