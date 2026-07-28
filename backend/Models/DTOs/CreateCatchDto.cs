using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.DTOs;
public record CreateCatchDto(
    int SpeciesId,
    int? SpotId,
    int? BaitId,
    double Latitude,
    double Longitude,
    DateTimeOffset CaughtAt,
    double? WeightLbs,
    WeightMethod? WeightMethod,
    double? LengthInches,
    double? WaterTempF,
    string? Notes
);