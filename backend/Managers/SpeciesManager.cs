using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers;
public class SpeciesManager(ISpeciesAccessor speciesAccessor) : ISpeciesManager
{
    private readonly ISpeciesAccessor _speciesAccessor = speciesAccessor;

    public async Task<List<Species>> GetAllAsync()
    {
        return await _speciesAccessor.GetAllAsync(); 
    }

    public async Task<Species?> GetByIdAsync(int id)
    {
        return await _speciesAccessor.GetByIdAsync(id);
    }
}
