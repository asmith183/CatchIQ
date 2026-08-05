using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Exceptions;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers;
public class SpotManager(ISpotAccessor spotAccessor) : ISpotManager
{
    private readonly ISpotAccessor _spotAccessor = spotAccessor;

    public async Task<List<SpotResponseDto>> GetAllByUserAsync(int userId)
    {
        var spots = await _spotAccessor.GetAllByUserAsync(userId);
        return spots.Select(MapToDto).ToList();
    }
    
    public async Task<SpotResponseDto?> GetByIdAsync(int spotId, int userId)
    {
        var spot = await _spotAccessor.GetByIdAsync(spotId, userId);

        if (spot == null) 
            return null;

        return MapToDto(spot);
    }

    public async Task<SpotResponseDto> CreateAsync(CreateSpotDto createSpotDto, int userId)
    {
        var spotEntity = new Spot
        {
            UserId = userId,
            Name = createSpotDto.Name,
            Latitude = createSpotDto.Latitude,
            Longitude = createSpotDto.Longitude,
            WaterBodyType = createSpotDto.WaterBodyType,
            Notes = createSpotDto.Notes
        };

        var createdSpot = await _spotAccessor.CreateAsync(spotEntity);
        return MapToDto(createdSpot);
    }

    public async Task<SpotResponseDto?> UpdateAsync(int spotId, UpdateSpotDto updateSpotDto, int userId)
    {
        var spotEntity = new Spot
        {
            Id = spotId,
            UserId = userId,
            Name = updateSpotDto.Name,
            Latitude = updateSpotDto.Latitude,
            Longitude = updateSpotDto.Longitude,
            WaterBodyType = updateSpotDto.WaterBodyType,
            Notes = updateSpotDto.Notes
        };

        var updated = await _spotAccessor.UpdateAsync(spotEntity);

        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var existing = await _spotAccessor.GetByIdAsync(id, userId);
        if (existing == null)
            return false;

        var hasCatches = await _spotAccessor.HasCatchesAsync(id);
        if (hasCatches)
            throw new SpotHasCatchesException(id);

        return await _spotAccessor.DeleteAsync(id, userId);
    }

    #region Helper Methods
    private static SpotResponseDto MapToDto(Spot spotEntity)
    {
        return new SpotResponseDto(
            Id: spotEntity.Id,
            UserId: spotEntity.UserId,
            Name: spotEntity.Name,
            Latitude: spotEntity.Latitude,
            Longitude: spotEntity.Longitude,
            WaterBodyType: spotEntity.WaterBodyType,
            Notes: spotEntity.Notes
        );
    }
    #endregion
}