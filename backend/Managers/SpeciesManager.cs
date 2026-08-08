using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers;
public class SpeciesManager(ISpeciesAccessor speciesAccessor) : ISpeciesManager
{
    private readonly ISpeciesAccessor _speciesAccessor = speciesAccessor;

    public async Task<List<SpeciesResponseDto>> GetAllAsync()
    {
        var species = await _speciesAccessor.GetAllAsync();

        return species.Select(MapToDto).ToList();
    }

    public async Task<SpeciesResponseDto?> GetByIdAsync(int id)
    {
        var species = await _speciesAccessor.GetByIdAsync(id);

        if (species == null)
            return null;

        return MapToDto(species);
    }

    #region Helper Methods
    private static SpeciesResponseDto MapToDto(Species speciesEntity)
    {
        return new SpeciesResponseDto(
            Id: speciesEntity.Id,
            Name: speciesEntity.Name
        );
    }
    #endregion
}
