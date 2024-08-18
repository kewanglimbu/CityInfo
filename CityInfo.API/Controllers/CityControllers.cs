using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/Cities")]
    public class CityControllers: ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            IEnumerable<CityDto> cities = CityDataStore.Current.Cities;
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public ActionResult GetCity(int id) 
        {
            IEnumerable<CityDto> cities = CityDataStore.Current.Cities;
            var city = cities.FirstOrDefault(x => x.Id == id);
            return Ok(city);
        }

    }
}
