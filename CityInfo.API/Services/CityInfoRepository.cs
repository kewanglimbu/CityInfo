using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestForCityAsync(int cityId)
        {
           return await _dbContext.PointOfInterests.Where(p =>p.CityId == cityId).ToListAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _dbContext.PointOfInterests.Where(p =>p.CityId==cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsCityExistAsync(int cityId)
        {
            return await _dbContext.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            //var cityToAddPointOfInterest = await _dbContext.Cities.Where(p => p.Id == cityId ).FirstOrDefaultAsync();
            var cityToAddPointOfInterest = await GetCityAsync(cityId, false);
            if (cityToAddPointOfInterest != null)
            {
                cityToAddPointOfInterest.PointOfInterest.Add(pointOfInterest);
                await _dbContext.SaveChangesAsync();
            }
        }

        public void DeletePointOfInterestForCity(PointOfInterest pointOfInterest)
        {
               _dbContext.PointOfInterests.Remove(pointOfInterest);
        }

        public async Task DeleteAllPointOfInterestForCityAsync(int cityId)
        {
            var pointOfInterestToDeleteForCity = await _dbContext.PointOfInterests.Where(p => p.CityId== cityId).ToListAsync();
            if(pointOfInterestToDeleteForCity.Any())
                _dbContext.PointOfInterests.RemoveRange(pointOfInterestToDeleteForCity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
