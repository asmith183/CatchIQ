using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Data;
using CatchIQ.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatchIQ.API.Accessors;
public class CatchAccessor(AppDbContext dbContext) : ICatchAccessor
{
    private readonly AppDbContext _dbContext = dbContext;

    /// <summary>
    /// Returns a list of all of the catches belonging to the specified user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Catch>> GetAllByUserAsync(int userId)
    {
        return await _dbContext.Catches
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Gets the catch with the given catchId and userId
    /// </summary>
    /// <param name="catchId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<Catch?> GetByIdAsync(int catchId, int userId)
    {
        return await _dbContext.Catches
            .Where(c => c.UserId == userId && c.Id == catchId)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Saves a catch to the database
    /// </summary>
    /// <param name="catchEntity"></param>
    /// <returns></returns>
    public async Task<Catch> CreateAsync(Catch catchEntity)
    {
        _dbContext.Catches.Add(catchEntity);
        await _dbContext.SaveChangesAsync();

        return catchEntity;
    }

    /// <summary>
    /// Updates a catch in the database
    /// </summary>
    /// <param name="catchEntity"></param>
    /// <returns></returns>
    public async Task<Catch?> UpdateAsync(Catch catchEntity)
    {
        var existing = await GetByIdAsync(catchEntity.Id, catchEntity.UserId);

        if (existing == null) 
            return null;

        existing.SpeciesId = catchEntity.SpeciesId;
        existing.SpotId = catchEntity.SpotId;
        existing.BaitId = catchEntity.BaitId;
        existing.Latitude = catchEntity.Latitude;
        existing.Longitude = catchEntity.Longitude;
        existing.CaughtAt = catchEntity.CaughtAt;
        existing.WeightLbs = catchEntity.WeightLbs;
        existing.WeightMethod = catchEntity.WeightMethod;
        existing.LengthInches = catchEntity.LengthInches;
        existing.AirTempF = catchEntity.AirTempF;
        existing.WindSpeedMph = catchEntity.WindSpeedMph;
        existing.WindDirection = catchEntity.WindDirection;
        existing.PressureMb = catchEntity.PressureMb;
        existing.SkyCondition = catchEntity.SkyCondition;
        existing.WaterTempF = catchEntity.WaterTempF;
        existing.Notes = catchEntity.Notes;
        existing.PhotoUrl = catchEntity.PhotoUrl;
        existing.UpdatedAt = DateTimeOffset.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        return existing;
    }

    /// <summary>
    /// Deletes the catch with the given id and userId
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var existing = await GetByIdAsync(id, userId);

        if (existing == null) 
            return false;

        _dbContext.Remove(existing);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}