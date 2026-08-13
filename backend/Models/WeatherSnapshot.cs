using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models;
public record WeatherSnapshot(
    double? AirTempF,
    double? WindSpeedMph,
    WindDirection? WindDirection,
    double? PressureMb,
    SkyCondition? SkyCondition
);
