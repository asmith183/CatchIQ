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
public class CatchController(ICatchManager catchManager) : ControllerBase
{
    private readonly ICatchManager _catchManager = catchManager;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _catchManager.GetAllByUserAsync(GetUserId());

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _catchManager.GetByIdAsync(id, GetUserId());

        if (result == null) 
            return NotFound();
        else 
            return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCatchDto createCatchDto)
    {
        try
        {
            var result = await _catchManager.CreateAsync(createCatchDto, GetUserId());

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (SpeciesNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCatchDto updateCatchDto)
    {
        try
        {
            var result = await _catchManager.UpdateAsync(id, updateCatchDto, GetUserId());

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        catch (SpeciesNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _catchManager.DeleteAsync(id, GetUserId());

        if (!deleted) 
            return NotFound();
        else 
            return NoContent();
    }

    #region Helper Methods
    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    #endregion
}