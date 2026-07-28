using CatchIQ.API.Accessors.Interfaces;
using CatchIQ.API.Data;
using CatchIQ.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatchIQ.API.Accessors;
public class SpeciesAccessor(AppDbContext context) : ISpeciesAccessor
{
    private readonly AppDbContext _context = context;

    public async Task<List<Species>> GetAllAsync()
    {
        return await _context.Species.ToListAsync();
    }

    public async Task<Species?> GetByIdAsync(int id)
    {
        return await _context.Species.FindAsync(id);
    }
}
