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
public class BaitController(IBaitManager baitManager) : ControllerBase
{
    private readonly IBaitManager _baitManager = baitManager;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _baitManager.GetAllByUserAsync(GetUserId());

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _baitManager.GetByIdAsync(id, GetUserId());

        if (result == null)
            return NotFound();
        else
            return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBaitDto createBaitDto)
    {
        var result = await _baitManager.CreateAsync(createBaitDto, GetUserId());

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBaitDto updateBaitDto)
    {
        var result = await _baitManager.UpdateAsync(id, updateBaitDto, GetUserId());

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
            var deleted = await _baitManager.DeleteAsync(id, GetUserId());

            if (!deleted)
                return NotFound();
            else
                return NoContent();
        }
        catch (BaitHasCatchesException ex)
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
