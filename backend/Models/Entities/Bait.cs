using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Models.Entities;
public class Bait
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Brand { get; set; }
    public string Color { get; set; } = null!;
    public BaitType Type { get; set; }
    public string?  Notes { get; set; }
}