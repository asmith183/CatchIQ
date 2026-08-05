using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers.Interfaces;
public interface ISpotManager
{
    Task<List<SpotResponseDto>> GetAllByUserAsync(int userId);
    Task<SpotResponseDto?> GetByIdAsync(int spotId, int userId);
    Task<SpotResponseDto> CreateAsync(CreateSpotDto createSpotDto, int userId);
    Task<SpotResponseDto?> UpdateAsync(int spotId, UpdateSpotDto updateSpotDto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}