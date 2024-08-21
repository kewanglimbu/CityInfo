using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointofinterest")]
    public class PointOfInterestController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId)
        {
            var cities = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cities == null)
            {
                return NotFound();
            }
            return Ok(cities.PointOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = CityDataStore.Current.Cities.SelectMany(c => c.PointOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            city.PointOfInterest.Add(finalPointOfInterest);


            return CreatedAtRoute(
                "GetPointOfInterest", // Route name for retrieving the specific point of interest
                new
                {
                    cityId = cityId,
                    pointOfInterestId = finalPointOfInterest.Id,
                },
                finalPointOfInterest);
        }

    }
}
