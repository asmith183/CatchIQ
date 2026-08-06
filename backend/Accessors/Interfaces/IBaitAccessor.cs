using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Accessors.Interfaces;
public interface IBaitAccessor
{
    Task<List<Bait>> GetAllByUserAsync(int userId);
    Task<Bait?> GetByIdAsync(int baitId, int userId);
    Task<Bait> CreateAsync(Bait baitEntity);
    Task<Bait?> UpdateAsync(Bait baitEntity);
    Task<bool> DeleteAsync(int baitId, int userId);
    Task<bool> HasCatchesAsync(int baitId);
}
