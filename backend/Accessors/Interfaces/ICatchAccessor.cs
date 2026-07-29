using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Accessors.Interfaces;
public interface ICatchAccessor
{
    Task<List<Catch>> GetAllByUserAsync(int userId);
    Task<Catch?> GetByIdAsync(int catchId, int userId);
    Task<Catch> CreateAsync(Catch catchEntity);
    Task<Catch?> UpdateAsync(Catch catchEntity);
    Task<bool> DeleteAsync(int id, int userId);
}
