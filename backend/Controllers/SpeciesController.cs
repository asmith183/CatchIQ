using CatchIQ.API.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchIQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SpeciesController(ISpeciesManager speciesManager) : ControllerBase
{
    private readonly ISpeciesManager _speciesManager = speciesManager;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _speciesManager.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _speciesManager.GetByIdAsync(id);

        if (result == null)
            return NotFound();
        else
            return Ok(result);
    }
}