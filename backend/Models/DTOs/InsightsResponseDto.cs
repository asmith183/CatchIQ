namespace CatchIQ.API.Models.DTOs;
public record InsightsResponseDto(
    string BestTimeOfDay,
    string BestBait,
    string BestConditions,
    string Summary,
    DateTimeOffset GeneratedAt
);
