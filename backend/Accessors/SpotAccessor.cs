using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Accessors;
public class SpotAccessor : ISpotAccessor
{
    public async Task<List<Spot>> GetAllByUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Spot?> GetByIdAsync(int spotId, int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Spot> CreateAsync(Spot spotEntity)
    {
        throw new NotImplementedException();
    }

    public async Task<Spot?> UpdateAsync(Spot spotEntity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int spotId, int userId)
    {
        throw new NotImplementedException();
    }

}