using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Managers.Interfaces;
public interface ICatchManager
{
    Task<List<CatchResponseDto>> GetAllByUserAsync(int userId);
    Task<CatchResponseDto?> GetByIdAsync(int catchId, int userId);
    Task<CatchResponseDto> CreateAsync(CreateCatchDto createCatchDto, int userId);
    Task<CatchResponseDto?> UpdateAsync(int catchId, UpdateCatchDto updateCatchDto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}