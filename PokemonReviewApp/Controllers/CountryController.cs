using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepositry _countryRepositry;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepositry countryRepositry, IMapper mapper)
        {
            _countryRepositry = countryRepositry;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepositry.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(countries);
        }

        [HttpGet("{CountryID}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int CountryID)
        {
            if(!_countryRepositry.CountryExist(CountryID))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepositry.GetCountry(CountryID));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        [HttpGet("/owners/{ownerID}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfOwner(int ownerID)
        {
            var country = _mapper.Map<CountryDto>(_countryRepositry.GetCountryByOwner(ownerID));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }
    }
}
