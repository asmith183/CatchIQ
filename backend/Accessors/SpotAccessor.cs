using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Data;
using CatchIQ.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatchIQ.API.Accessors;
public class SpotAccessor(AppDbContext dbContext) : ISpotAccessor
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Spot>> GetAllByUserAsync(int userId)
    {
        return await _dbContext.Spots
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Spot?> GetByIdAsync(int spotId, int userId)
    {
        return await _dbContext.Spots
            .Where(x => x.Id == spotId && x.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Spot> CreateAsync(Spot spotEntity)
    {
        _dbContext.Spots.Add(spotEntity);
        await _dbContext.SaveChangesAsync();

        return spotEntity;
    }

    public async Task<Spot?> UpdateAsync(Spot spotEntity)
    {
        var existing = await GetByIdAsync(spotEntity.Id, spotEntity.UserId);

        if (existing == null)
            return null;

        existing.Name = spotEntity.Name;
        existing.Latitude = spotEntity.Latitude;
        existing.Longitude = spotEntity.Longitude;
        existing.WaterBodyType = spotEntity.WaterBodyType;
        existing.Notes = spotEntity.Notes;

        await _dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int spotId, int userId)
    {
        var existing = await GetByIdAsync(spotId, userId);

        if (existing == null) 
            return false;

        _dbContext.Remove(existing);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> HasCatchesAsync(int spotId)
    {
        return await _dbContext.Catches.AnyAsync(c => c.SpotId == spotId);
    }
}