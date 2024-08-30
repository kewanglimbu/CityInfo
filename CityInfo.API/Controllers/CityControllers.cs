using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/Cities")]
    public class CityControllers : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int MAX_CITIES_PAGESIZE = 20;

        public CityControllers(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            var cities = await _cityInfoRepository.GetAllCitiesAsync();
            //var cityWithoutPOIDto = new List<CityWithoutPointOfInterestDto>();
            //foreach (var city in cities)
            //{
            //    cityWithoutPOIDto.Add(new CityWithoutPointOfInterestDto
            //    {
            //        Id = city.Id,
            //        Name = city.Name,
            //        Description = city.Description,
            //    });
            //}
            var result = _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cities);
            return Ok(result);
        }

        [HttpGet("filterbyname")]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCitiesByFiltering(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var filerCities = await _cityInfoRepository.GetCitiesByFilteringAsync(name);
            var result = _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(filerCities);
            return Ok(filerCities);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> SearchForQueryInCity(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            //if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(searchQuery))
            //    return BadRequest("At least one search parameter (name or searchQuery) must be provided.");

            if(pageSize > MAX_CITIES_PAGESIZE)
            {
                pageSize = MAX_CITIES_PAGESIZE;
            }
            var filerCities = await _cityInfoRepository.SearchQueryForCitiesAsync(name, searchQuery,pageNumber, pageSize);
            var result = _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(filerCities);
            return Ok(filerCities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);
            if (city == null)
                return NotFound();

            if (includePointOfInterest)
            {
                var result = _mapper.Map<CityDto>(city);
                return Ok(result);
            }
            else
            {
                var result = _mapper.Map<CityWithoutPointOfInterestDto>(city);
                return Ok(result);
            }
        }
    }
}
