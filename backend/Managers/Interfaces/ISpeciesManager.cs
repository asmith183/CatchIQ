using CatchIQ.API.Models.DTOs;

namespace CatchIQ.API.Managers.Interfaces;
public interface ISpeciesManager
{
    Task<List<SpeciesResponseDto>> GetAllAsync();
    Task<SpeciesResponseDto?> GetByIdAsync(int id);
}
