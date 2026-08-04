using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Accessors.Interfaces;
public interface ISpotAccessor
{
    Task<List<Spot>> GetAllByUserAsync(int userId);
    Task<Spot?> GetByIdAsync(int spotId, int userId);
    Task<Spot> CreateAsync(Spot spotEntity);
    Task<Spot?> UpdateAsync(Spot spotEntity);
    Task<bool> DeleteAsync(int spotId, int userId);
}