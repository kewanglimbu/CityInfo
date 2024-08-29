using AutoMapper;
using CityInfo.API.Entities;
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
        private ICityInfoRepository _cityInfoRepository;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentException(nameof(cityInfoRepository));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterest(int cityId)
        {
            try
            {
                if (!await _cityInfoRepository.IsCityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }
                var pointsOfInterestForCity = await _cityInfoRepository.GetAllPointOfInterestForCityAsync(cityId);

                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while retrieving Points of interest for city with Id {cityId}.{ex}", ex);
                return StatusCode(500, "An error occurred while processing your request. ");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                if (!await _cityInfoRepository.IsCityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                var pointOfInterestForCity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

                if (pointOfInterestForCity == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterestForCity));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while retrieving point of interest for city with Id {cityId}.", ex);
                return StatusCode(500, "An error occurred while processing your request. ");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);
                await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
                await _cityInfoRepository.SaveChangesAsync();

                var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

                return CreatedAtRoute(
                    "GetPointOfInterest", // Route name for retrieving the specific point of interest
                    new
                    {
                        cityId = cityId,
                        pointOfInterestId = createdPointOfInterestToReturn.Id
                    },
                    createdPointOfInterestToReturn
                   );
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a point of interest for city with Id {cityId}: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (!await _cityInfoRepository.IsCityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                var cityToUpdatePOI = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
                if (cityToUpdatePOI == null)
                {
                    return NotFound();
                }
                _mapper.Map(pointOfInterest, cityToUpdatePOI);
                await _cityInfoRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while Updating a point of interest for city with Id {cityId}: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartialUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            try
            {
                if (!await _cityInfoRepository.IsCityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                var cityToUpdatePOI = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
                if (cityToUpdatePOI == null)
                {
                    return NotFound();
                }

                var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(cityToUpdatePOI);

                patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!TryValidateModel(pointOfInterestToPatch))
                {
                    return BadRequest(ModelState);
                }
                _mapper.Map(pointOfInterestToPatch, cityToUpdatePOI);
                await _cityInfoRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while Partial Updating a point of interest for city with Id {cityId}: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                if (!await _cityInfoRepository.IsCityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                var pointOfInterestInCity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

                if (pointOfInterestInCity == null)
                {
                    _logger.LogInformation($"PointOfInterest with Id {pointOfInterestId} was not found.");
                    return NotFound();
                }
                _cityInfoRepository.DeletePointOfInterestForCity(pointOfInterestInCity);
                await _cityInfoRepository.SaveChangesAsync();

                await _mailService.Send("PointOfInterest Deleted", $"PointOfInterest '{pointOfInterestInCity.Name}' with Id {pointOfInterestInCity.Id} is deleted.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while retrieving points of interest for city with Id {cityId}.", ex);
                return StatusCode(500, "An error occurred while processing your request. ");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllPointOfInterest(int cityId)
        {
            try
            {
                if (!await _cityInfoRepository.IsCityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found.");
                    return NotFound();
                }

                var pointOfInterestInCity = await _cityInfoRepository.GetAllPointOfInterestForCityAsync(cityId);

                if (pointOfInterestInCity == null || !pointOfInterestInCity.Any())
                {
                    _logger.LogInformation($"City with Id {cityId} do not contains PointOfInterest.");
                    return NotFound(new { message = "No Points of Interest to delete for the specified city." });
                }
                await _cityInfoRepository.DeleteAllPointOfInterestForCityAsync(cityId);
                await _cityInfoRepository.SaveChangesAsync();

                await _mailService.Send("Deleted All PointOfInterest ", $"All PointOfInterestCity is deleted with City Id {cityId}.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while retrieving points of interest for city with Id for all delete POI. {cityId}.", ex);
                return StatusCode(500, "An error occurred while processing your request. ");
            }
        }
    }
}
