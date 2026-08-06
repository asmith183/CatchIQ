using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.DTOs;
public record UpdateBaitDto(
    string Name,
    string? Brand,
    string? Color,
    string? Size,
    BaitType Type,
    RigStyle? RigStyle,
    string? Trailer,
    string? Notes
);
