using Microsoft.AspNetCore.Identity;

namespace CatchIQ.API.Models.Entities;
public class ApplicationUser : IdentityUser<int>
{
    public ICollection<Catch> Catches { get; set; } = new List<Catch>();
    public ICollection<Spot> Spots { get; set; } = new List<Spot>();
    public ICollection<Bait> Baits { get; set; } = new List<Bait>();
}