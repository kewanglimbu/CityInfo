using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoDbContext: DbContext
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }
    }
}
