using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/Cities")]
    public class CityControllers: ControllerBase
    {
        private readonly CityDataStore _cityDataStore;

        public CityControllers(CityDataStore cityDataStore)
        {
            _cityDataStore = cityDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            IEnumerable<CityDto> cities = _cityDataStore.Cities;
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public ActionResult GetCity(int id) 
        {
            IEnumerable<CityDto> cities = _cityDataStore.Cities;
            var city = cities.FirstOrDefault(x => x.Id == id);
            return Ok(city);
        }

    }
}
