using ExternalDataRetrieverService.Models;
using Microsoft.EntityFrameworkCore;

namespace EarthquakeRetrieverService.Models;

public class EarthquakeContext : DbContext
{
    public EarthquakeContext(DbContextOptions<EarthquakeContext> options)
        : base(options)
    {
    }

    public DbSet<Earthquake> Earthquakes { get; set; } = null!;
}