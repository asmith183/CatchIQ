using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Data;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Accessors;
public class CatchAccessor(AppDbContext dbContext) : ICatchAccessor
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Catch>> GetAllByUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Catch?> GetByIdAsync(int catchId)
    {
        throw new NotImplementedException();
    }

    public async Task<Catch> CreateAsync(Catch catchEntity)
    {
        throw new NotImplementedException();
    }

    public async Task<Catch?> UpdateAsync(Catch catchEntity)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}