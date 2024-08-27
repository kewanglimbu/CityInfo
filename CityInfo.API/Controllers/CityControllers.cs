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
