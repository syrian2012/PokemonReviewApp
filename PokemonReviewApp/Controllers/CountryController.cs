using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

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
                return BadRequest(ModelState);

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
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("/owners/{ownerID}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfOwner(int ownerID)
        {
            var country = _mapper.Map<CountryDto>(_countryRepositry.GetCountryByOwner(ownerID));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CreateCountryDto createCountry)
        {
            if (createCountry == null)
                return BadRequest(ModelState);

            var country = _countryRepositry.GetCountries()
                .Where(c => c.Name.Trim().ToLower() == createCountry.Name.Trim().ToLower()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country Already Exists");
                return StatusCode(422,ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(createCountry);

            if (!_countryRepositry.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Saving Your Entry");
                return StatusCode(500, ModelState);
            }

            return Ok("Country Created Successfully");
        }

        [HttpPut("{CountryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int CountryID, [FromBody] CreateCountryDto updateCountry)
        {
            if (updateCountry == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepositry.CountryExist(CountryID))
                return NotFound();

            var countryMap = _mapper.Map<Country>(updateCountry);
            countryMap.ID = CountryID;

            if (!_countryRepositry.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Updating Your Entry");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
