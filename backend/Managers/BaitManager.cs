using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Exceptions;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Managers;
public class BaitManager(IBaitAccessor baitAccessor) : IBaitManager
{
    private readonly IBaitAccessor _baitAccessor = baitAccessor;

    public async Task<List<BaitResponseDto>> GetAllByUserAsync(int userId)
    {
        var baits = await _baitAccessor.GetAllByUserAsync(userId);
        return baits.Select(MapToDto).ToList();
    }

    public async Task<BaitResponseDto?> GetByIdAsync(int baitId, int userId)
    {
        var bait = await _baitAccessor.GetByIdAsync(baitId, userId);

        if (bait == null)
            return null;

        return MapToDto(bait);
    }

    public async Task<BaitResponseDto> CreateAsync(CreateBaitDto createBaitDto, int userId)
    {
        var baitEntity = new Bait
        {
            UserId = userId,
            Name = createBaitDto.Name,
            Brand = createBaitDto.Brand,
            Color = createBaitDto.Color,
            Size = createBaitDto.Size,
            Type = createBaitDto.Type,
            RigStyle = createBaitDto.RigStyle,
            Trailer = createBaitDto.Trailer,
            Notes = createBaitDto.Notes
        };

        var createdBait = await _baitAccessor.CreateAsync(baitEntity);
        return MapToDto(createdBait);
    }

    public async Task<BaitResponseDto?> UpdateAsync(int baitId, UpdateBaitDto updateBaitDto, int userId)
    {
        var baitEntity = new Bait
        {
            Id = baitId,
            UserId = userId,
            Name = updateBaitDto.Name,
            Brand = updateBaitDto.Brand,
            Color = updateBaitDto.Color,
            Size = updateBaitDto.Size,
            Type = updateBaitDto.Type,
            RigStyle = updateBaitDto.RigStyle,
            Trailer = updateBaitDto.Trailer,
            Notes = updateBaitDto.Notes
        };

        var updated = await _baitAccessor.UpdateAsync(baitEntity);

        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var existing = await _baitAccessor.GetByIdAsync(id, userId);
        if (existing == null)
            return false;

        var hasCatches = await _baitAccessor.HasCatchesAsync(id);
        if (hasCatches)
            throw new BaitHasCatchesException(id);

        return await _baitAccessor.DeleteAsync(id, userId);
    }

    #region Helper Methods
    private static BaitResponseDto MapToDto(Bait baitEntity)
    {
        return new BaitResponseDto(
            Id: baitEntity.Id,
            UserId: baitEntity.UserId,
            Name: baitEntity.Name,
            Brand: baitEntity.Brand,
            Color: baitEntity.Color,
            Size: baitEntity.Size,
            Type: baitEntity.Type,
            RigStyle: baitEntity.RigStyle,
            Trailer: baitEntity.Trailer,
            Notes: baitEntity.Notes
        );
    }
    #endregion
}
