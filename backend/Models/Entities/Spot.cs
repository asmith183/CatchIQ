using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.Entities;
public class Spot
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public WaterBodyType WaterBodyType { get; set; }
    public string? Notes { get; set; } 
    public ICollection<Catch> Catches { get; set; } = new List<Catch>();
}