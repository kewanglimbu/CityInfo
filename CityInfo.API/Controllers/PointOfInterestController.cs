using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointofinterest")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private CityDataStore _cityDataStore;
        private readonly IMailService _mailService;

        public PointOfInterestController(ILogger<PointOfInterestController> logger,IMailService mailService, CityDataStore cityDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cityDataStore = cityDataStore;
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId)
        {
            //Throw exception just for testing purpose
            //throw new Exception("error ocuur");

            try
            {
                var cities = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (cities == null)
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                return Ok(cities.PointOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while retrieving points of interest for city with Id {cityId}.", ex);
                return StatusCode(500, "An error occurred while processing your request. ");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

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

            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = _cityDataStore.Cities.SelectMany(c => c.PointOfInterest).Max(p => p.Id);

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

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId ,PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestInCity = city.PointOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestInCity == null)
            {
                return NotFound();
            }
            pointOfInterestInCity.Name = pointOfInterest.Name;
            pointOfInterestInCity.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartialUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c=>c.Id== cityId);
            if(city == null)
            {
                return NotFound();
            }

            var pointOfInterestInCity = city.PointOfInterest.FirstOrDefault(c=>c.Id == pointOfInterestId);
            if(pointOfInterestInCity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = pointOfInterestInCity.Name,
                Description = pointOfInterestInCity.Description,
            };

            patchDocument.ApplyTo(pointOfInterestToPatch,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            // Apply the changes back to the original point of interest
            pointOfInterestInCity.Name = pointOfInterestToPatch.Name;
            pointOfInterestInCity.Description= pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                var pointOfInterestInCity = city.PointOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
                if (pointOfInterestInCity == null)
                {
                    _logger.LogInformation($"PointOfInterest with Id {pointOfInterestId} was not found.");
                    return NotFound();
                }

                city.PointOfInterest.Remove(pointOfInterestInCity);
                _mailService.Send("PointOfInterest Deleted",$"PointOfInterest '{pointOfInterestInCity.Name}' with Id {pointOfInterestInCity.Id} is deleted.");
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while retrieving points of interest for city with Id {cityId}.", ex);
                return StatusCode(500, "An error occurred while processing your request. ");
            }
        }
    }
}
