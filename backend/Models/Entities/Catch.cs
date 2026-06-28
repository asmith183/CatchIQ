using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.Entities;
public class Catch
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public int? SpotId { get; set; }
    public Spot? Spot { get; set; }
    public int SpeciesId { get; set; }
    public Species Species { get; set; } = null!;
    public int? BaitId { get; set; } 
    public Bait? Bait { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; } 
    public DateTimeOffset CaughtAt { get; set; }
    public double? WeightLbs { get; set; }
    public WeightMethod? WeightMethod { get; set; }
    public double? LengthInches { get; set; }
    public double? AirTempF { get; set; }
    public double? WindSpeedMph { get; set; }
    public WindDirection? WindDirection { get; set; }
    public double? PressureMb { get; set; }
    public SkyCondition? SkyCondition { get; set; } 
    public double? WaterTempF { get; set; }
    public string? Notes { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}