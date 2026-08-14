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
public class InsightsController(IInsightsManager insightsManager) : ControllerBase
{
    private readonly IInsightsManager _insightsManager = insightsManager;

    [HttpGet]
    [ProducesResponseType(typeof(InsightsResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InsightsResponseDto>> Get()
    {
        try
        {
            var result = await _insightsManager.GetForUserAsync(GetUserId());

            return Ok(result);
        }
        catch (InsufficientCatchDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InsightsGenerationFailedException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
    }

    #region Helper Methods
    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    #endregion
}
