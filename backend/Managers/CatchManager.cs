using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Exceptions;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers;
public class CatchManager(ICatchAccessor catchAccessor, ISpeciesAccessor speciesAccessor, ISpotAccessor spotAccessor, IBaitAccessor baitAccessor) : ICatchManager
{
    private readonly ICatchAccessor _catchAccessor = catchAccessor;
    private readonly ISpeciesAccessor _speciesAccessor = speciesAccessor;
    private readonly ISpotAccessor _spotAccessor = spotAccessor;
    private readonly IBaitAccessor _baitAccessor = baitAccessor;

    public async Task<List<CatchResponseDto>> GetAllByUserAsync(int userId)
    {
        var catches = await _catchAccessor.GetAllByUserAsync(userId);
        return catches.Select(MapToDto).ToList();
    }

    public async Task<CatchResponseDto?> GetByIdAsync(int catchId, int userId)
    {
        var existing = await _catchAccessor.GetByIdAsync(catchId, userId);

        if (existing == null)
            return null;
        
        return MapToDto(existing);
    }

    public async Task<CatchResponseDto> CreateAsync(CreateCatchDto createCatchDto, int userId)
    {
        var species = await _speciesAccessor.GetByIdAsync(createCatchDto.SpeciesId);
        if (species == null)
            throw new SpeciesNotFoundException(createCatchDto.SpeciesId);

        if (createCatchDto.SpotId.HasValue)
        {
            var spot = await _spotAccessor.GetByIdAsync(createCatchDto.SpotId.Value, userId);
            if (spot == null)
                throw new SpotNotFoundException(createCatchDto.SpotId.Value);
        }

        if (createCatchDto.BaitId.HasValue)
        {
            var bait = await _baitAccessor.GetByIdAsync(createCatchDto.BaitId.Value, userId);
            if (bait == null)
                throw new BaitNotFoundException(createCatchDto.BaitId.Value);
        }

        var catchEntity = new Catch
        {
            UserId = userId,
            SpeciesId = createCatchDto.SpeciesId,
            SpotId = createCatchDto.SpotId,
            BaitId = createCatchDto.BaitId,
            Latitude = createCatchDto.Latitude,
            Longitude = createCatchDto.Longitude,
            CaughtAt = createCatchDto.CaughtAt,
            WeightLbs = createCatchDto.WeightLbs,
            WeightMethod = createCatchDto.WeightMethod,
            LengthInches = createCatchDto.LengthInches,
            WaterTempF = createCatchDto.WaterTempF,
            Notes = createCatchDto.Notes
        };

        var createdCatch = await _catchAccessor.CreateAsync(catchEntity);
        return MapToDto(createdCatch);
    }
    
    public async Task<CatchResponseDto?> UpdateAsync(int catchId, UpdateCatchDto updateCatchDto, int userId)
    {
        var species = await _speciesAccessor.GetByIdAsync(updateCatchDto.SpeciesId);
        if (species == null)
            throw new SpeciesNotFoundException(updateCatchDto.SpeciesId);

        if (updateCatchDto.SpotId.HasValue)
        {
            var spot = await _spotAccessor.GetByIdAsync(updateCatchDto.SpotId.Value, userId);
            if (spot == null)
                throw new SpotNotFoundException(updateCatchDto.SpotId.Value);
        }

        if (updateCatchDto.BaitId.HasValue)
        {
            var bait = await _baitAccessor.GetByIdAsync(updateCatchDto.BaitId.Value, userId);
            if (bait == null)
                throw new BaitNotFoundException(updateCatchDto.BaitId.Value);
        }

        var catchEntity = new Catch
        {
            Id = catchId,
            UserId = userId,
            SpeciesId = updateCatchDto.SpeciesId,
            SpotId = updateCatchDto.SpotId,
            BaitId = updateCatchDto.BaitId,
            Latitude = updateCatchDto.Latitude,
            Longitude = updateCatchDto.Longitude,
            CaughtAt = updateCatchDto.CaughtAt,
            WeightLbs = updateCatchDto.WeightLbs,
            WeightMethod = updateCatchDto.WeightMethod,
            LengthInches = updateCatchDto.LengthInches,
            WaterTempF = updateCatchDto.WaterTempF,
            Notes = updateCatchDto.Notes
        };

        var updated = await _catchAccessor.UpdateAsync(catchEntity);

        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        return await _catchAccessor.DeleteAsync(id, userId);
    }

    #region Helper Methods
    private static CatchResponseDto MapToDto(Catch catchEntity)
    {
        return new CatchResponseDto(
            Id: catchEntity.Id,
            UserId: catchEntity.UserId,
            SpeciesId: catchEntity.SpeciesId,
            SpotId: catchEntity.SpotId,
            BaitId: catchEntity.BaitId,
            Latitude: catchEntity.Latitude,
            Longitude: catchEntity.Longitude,
            CaughtAt: catchEntity.CaughtAt,
            WeightLbs: catchEntity.WeightLbs,
            WeightMethod: catchEntity.WeightMethod,
            LengthInches: catchEntity.LengthInches,
            AirTempF: catchEntity.AirTempF,
            WindSpeedMph: catchEntity.WindSpeedMph,
            WindDirection: catchEntity.WindDirection,
            PressureMb: catchEntity.PressureMb,
            SkyCondition: catchEntity.SkyCondition,
            WaterTempF: catchEntity.WaterTempF,
            Notes: catchEntity.Notes,
            PhotoUrl: catchEntity.PhotoUrl,
            CreatedAt: catchEntity.CreatedAt,
            UpdatedAt: catchEntity.UpdatedAt
        );
    }
    #endregion
}