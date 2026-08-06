using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Data;
using CatchIQ.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatchIQ.API.Accessors;
public class BaitAccessor(AppDbContext dbContext) : IBaitAccessor
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Bait>> GetAllByUserAsync(int userId)
    {
        return await _dbContext.Baits
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Bait?> GetByIdAsync(int baitId, int userId)
    {
        return await _dbContext.Baits
            .Where(x => x.Id == baitId && x.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Bait> CreateAsync(Bait baitEntity)
    {
        _dbContext.Baits.Add(baitEntity);
        await _dbContext.SaveChangesAsync();

        return baitEntity;
    }

    public async Task<Bait?> UpdateAsync(Bait baitEntity)
    {
        var existing = await GetByIdAsync(baitEntity.Id, baitEntity.UserId);

        if (existing == null)
            return null;

        existing.Name = baitEntity.Name;
        existing.Brand = baitEntity.Brand;
        existing.Color = baitEntity.Color;
        existing.Size = baitEntity.Size;
        existing.Type = baitEntity.Type;
        existing.RigStyle = baitEntity.RigStyle;
        existing.Trailer = baitEntity.Trailer;
        existing.Notes = baitEntity.Notes;

        await _dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int baitId, int userId)
    {
        var existing = await GetByIdAsync(baitId, userId);

        if (existing == null)
            return false;

        _dbContext.Remove(existing);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> HasCatchesAsync(int baitId)
    {
        return await _dbContext.Catches.AnyAsync(c => c.BaitId == baitId);
    }
}
