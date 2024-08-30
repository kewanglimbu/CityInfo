using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);
        Task<IEnumerable<City>> GetCitiesByFilteringAsync(string name);
        Task<(IEnumerable<City>, PaginationMetadata)> SearchQueryForCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        Task<bool> IsCityExistAsync(int cityId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterestForCity(PointOfInterest pointOfInterest);
        Task DeleteAllPointOfInterestForCityAsync(int cityId);
        Task<bool> SaveChangesAsync(); 
    }
}
