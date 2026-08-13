using System.Security.Claims;
using CatchIQ.API.Exceptions;
using CatchIQ.API.Managers.Interfaces;
using CatchIQ.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatchIQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SpotController(ISpotManager spotManager) : ControllerBase
{
    private readonly ISpotManager _spotManager = spotManager;

    [HttpGet]
    public async Task<ActionResult<List<SpotResponseDto>>> GetAll()
    {
        var result = await _spotManager.GetAllByUserAsync(GetUserId());

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpotResponseDto>> GetById(int id)
    {
        var result = await _spotManager.GetByIdAsync(id, GetUserId());

        if (result == null)
            return NotFound();
        else
            return Ok(result);
    }

    // Documents the 201 in the OpenAPI spec; without it Swagger assumes 200
    // and the NSwag client treats the real 201 as an error.
    [HttpPost]
    [ProducesResponseType(typeof(SpotResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<SpotResponseDto>> Create(CreateSpotDto createSpotDto)
    {
        var result = await _spotManager.CreateAsync(createSpotDto, GetUserId());

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SpotResponseDto>> Update(int id, UpdateSpotDto updateSpotDto)
    {
        var result = await _spotManager.UpdateAsync(id, updateSpotDto, GetUserId());

        if (result == null)
            return NotFound();
        else
            return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _spotManager.DeleteAsync(id, GetUserId());

            if (!deleted)
                return NotFound();
            else
                return NoContent();
        }
        catch (SpotHasCatchesException ex)
        {
            return Conflict(ex.Message);
        }
    }

    #region Helper Methods
    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    #endregion
}