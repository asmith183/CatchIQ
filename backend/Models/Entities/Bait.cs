using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.Entities;
public class Bait
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public ApplicationUser User { get; set; } = null!; 
    public string Name { get; set; } = null!;
    public string? Brand { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public BaitType Type { get; set; }
    public RigStyle? RigStyle { get; set; }
    public string? Trailer { get; set; }
    public string? Notes { get; set; }
}