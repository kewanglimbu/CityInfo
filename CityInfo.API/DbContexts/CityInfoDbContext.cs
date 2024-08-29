using CityInfo.API.Entities;
using CityInfo.API.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("Gothgaun") { Id = 1, Description = "It is small peaceful semi urban area located in the morang district." },
                new City("Nakhipot") { Id = 2, Description = "It is the beautiful place located in the satdobato, lalitpur." },
                new City("Itahari") { Id = 3, Description = "It is city located in the Sunsari district and important business hub of eastern Nepal." },
                new City("Dharan") { Id = 4, Description = "It is city located in the Sunsari district and important business hub of eastern Nepal." }
                );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Purbanchal University") { Id= 1, Description = "The PU is situated here.", CityId = 1 },
                new PointOfInterest("Panchayat School") { Id= 2, Description = "The oldest government school in the gothgaun.", CityId = 1 },
                new PointOfInterest("Kirat Yakthung Chumlung") { Id = 3, Description = "Kirat Yakthung Chumlung is a social organization of the Limbu indigenous ethnic group of Nepal.",CityId= 2 },
                new PointOfInterest("Ullens School") { Id = 4, Description = "Ullens School is a private school in Nepal that offers holistic education from Kindergarten to IBDP.", CityId= 2 },
                new PointOfInterest("Bhatbhateni Store") { Id = 5, Description = "It is the biggest goods store in the Satdobato.",CityId= 2 },
                new PointOfInterest("Gorkha Department") { Id = 6, Description = "It is the oldest department store in the Itahari.",CityId= 3},
                new PointOfInterest("Namuna College") { Id = 7, Description = "It is a distinguished educational institution nestled in the heart of Itahari Sunsari.", CityId = 3 }
                );
        
            base.OnModelCreating(modelBuilder);
        }
    }
}
