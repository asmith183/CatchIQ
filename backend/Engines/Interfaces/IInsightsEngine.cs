using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Engines.Interfaces;
public interface IInsightsEngine
{
    Task<InsightsResponseDto?> GenerateAsync(string catchDataJson);
}
