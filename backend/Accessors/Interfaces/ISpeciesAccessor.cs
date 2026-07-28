using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Accessors.Interfaces;
public interface ISpeciesAccessor
{
    Task<List<Species>> GetAllAsync();
    Task<Species?> GetByIdAsync(int id);
}