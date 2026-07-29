using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers;
public class CatchManager(ICatchAccessor catchAccessor) : ICatchManager
{
    private readonly ICatchAccessor _catchAccessor = catchAccessor;

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