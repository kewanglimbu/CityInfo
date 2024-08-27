using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestForCity(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCity(int cityId, int pointOfInterestId);
    }
}
