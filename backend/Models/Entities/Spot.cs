using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.Entities;
public class Spot
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key to the ApplicationUser entity
    public ApplicationUser User { get; set; } = null!; // Navigation property to the ApplicationUser entity
    public string Name { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public WaterBodyType WaterBodyType { get; set; }
    public string? Notes { get; set; } 
}