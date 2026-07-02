using CatchIQ.API.Models.DTOs;
using CatchIQ.API.Models.Entities;

namespace CatchIQ.API.Engines.Interfaces;

public interface ITokenEngine
{
    public AuthResponseDto GenerateToken(ApplicationUser user);
}