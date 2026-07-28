using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers.Interfaces;
public interface ISpeciesManager
{
    Task<List<Species>> GetAllAsync();
    Task<Species?> GetByIdAsync(int id);
}