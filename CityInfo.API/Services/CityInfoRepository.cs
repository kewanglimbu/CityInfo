using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoDbContext _dbContext;

        public CityInfoRepository(CityInfoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _dbContext.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return await _dbContext.Cities.Include(c => c.PointOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _dbContext.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestForCity(int cityId)
        {
           return await _dbContext.PointOfInterests.Where(p =>p.Id == cityId).ToListAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return await _dbContext.PointOfInterests.Where(p =>p.CityId==cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }
    }
}
