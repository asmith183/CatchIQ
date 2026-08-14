using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Managers.Interfaces;
public interface IInsightsManager
{
    Task<InsightsResponseDto> GetForUserAsync(int userId);
}
