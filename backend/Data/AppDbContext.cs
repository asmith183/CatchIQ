using CatchIQ.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatchIQ.API.Data; 
public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Spot> Spots => Set<Spot>();
    public DbSet<Bait> Baits => Set<Bait>();
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Catch> Catches => Set<Catch>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ApplicationUser
        builder.Entity<ApplicationUser>(e =>
        {
            e.HasMany(u => u.Spots)
             .WithOne(s => s.User)
             .HasForeignKey(s => s.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(u => u.Baits)
             .WithOne(b => b.User)
             .HasForeignKey(b => b.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(u => u.Catches)
             .WithOne(c => c.User)
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Catch
        builder.Entity<Catch>(e =>
        {
            e.HasOne(c => c.Spot)
             .WithMany(s => s.Catches)
             .HasForeignKey(c => c.SpotId)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired(false);

            e.HasOne(c => c.Species)
             .WithMany(sp => sp.Catches)
             .HasForeignKey(c => c.SpeciesId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(c => c.Bait)
             .WithMany(b => b.Catches)
             .HasForeignKey(c => c.BaitId)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired(false);

            e.Property(c => c.WeightLbs)
             .HasColumnType("decimal(5,2)");
        });

        // Species Seed Data
        builder.Entity<Species>(e =>
        {
            e.HasData(
                new Species { Id = 1, Name = "Largemouth Bass" },
                new Species { Id = 2, Name = "Smallmouth Bass" },
                new Species { Id = 3, Name = "Channel Catfish" },
                new Species { Id = 4, Name = "Northern Pike" },
                new Species { Id = 5, Name = "Walleye" },
                new Species { Id = 6, Name = "Flathead Catfish" },
                new Species { Id = 7, Name = "Blue Catfish" },
                new Species { Id = 8, Name = "Freshwater Drum" },
                new Species { Id = 9, Name = "Bluegill" },
                new Species { Id = 10, Name = "Crappie" },
                new Species { Id = 11, Name = "Perch" },
                new Species { Id = 12, Name = "Carp" },
                new Species { Id = 13, Name = "Muskellunge" },
                new Species { Id = 14, Name = "Striped Bass" },
                new Species { Id = 15, Name = "White Bass" },
                new Species { Id = 16, Name = "Bowfin" }
            );
        });
    }
}