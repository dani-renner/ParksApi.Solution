using Microsoft.EntityFrameworkCore;

namespace Park.Models
{
  public class ParkContext : DbContext
  {
    public ParkContext(DbContextOptions<ParkContext> options)
      : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Park>()
      .HasData(
        new Park { ParkId = 1, Name = "Yellowstone", SqMiles = 3471, Location = "Wyoming, Montana, and Idaho" },
        new Park { ParkId = 2, Name = "Niagara Falls National Heritage Area", SqMiles = 1, Location = "Niagara Falls, Lewiston, and Porter, New York" },
        new Park { ParkId = 3, Name = "Yosemite National Park", SqMiles = 1169, Location = "California" },
        new Park { ParkId = 4, Name = "Glacier National Park", SqMiles = 1583, Location = "Montana" },
        new Park { ParkId = 5, Name = "Frontenac State Park", SqMiles = 4, Location = "Minnesota" }
      );
    }
    public DbSet<Park> Parks { get; set; }
  }
}