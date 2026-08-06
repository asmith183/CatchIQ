using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Managers.Interfaces;
public interface IBaitManager
{
    Task<List<BaitResponseDto>> GetAllByUserAsync(int userId);
    Task<BaitResponseDto?> GetByIdAsync(int baitId, int userId);
    Task<BaitResponseDto> CreateAsync(CreateBaitDto createBaitDto, int userId);
    Task<BaitResponseDto?> UpdateAsync(int baitId, UpdateBaitDto updateBaitDto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}
