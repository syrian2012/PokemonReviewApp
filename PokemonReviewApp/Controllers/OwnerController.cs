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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ICountryRepositry _countryRepositry;
        private readonly IMapper _mapper;


        public OwnerController(IOwnerRepository ownerRepository,IPokemonRepository pokemonRepository,ICountryRepositry countryRepositry ,IMapper mapper)
        {
            _ownerRepository= ownerRepository;
            _pokemonRepository = pokemonRepository;
            _countryRepositry = countryRepositry;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{OwnerID}")]
        [ProducesResponseType(200,Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int OwnerID)
        {
            if (!_ownerRepository.OwnerExists(OwnerID)) return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(OwnerID));

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{PokeID}/owners")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersOfAPokemon(int PokeID)
        {
            if (!_pokemonRepository.PokemonExists(PokeID)) return NotFound();

            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersOfAPokemon(PokeID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{OwnerID}/pokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByOwner(int OwnerID)
        {
            if (!_ownerRepository.OwnerExists(OwnerID)) return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(OwnerID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int CountryID,[FromBody] CreateOwnerDto createOwner)
        {
            if (createOwner == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(createOwner);

            ownerMap.Country = _countryRepositry.GetCountry(CountryID);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Saving Your Entry");
                return StatusCode(500, ModelState);
            }

            return Ok("Owner Created Successfully");
        }

        [HttpPut("{OwnerID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int OwnerID, [FromQuery] int CountryID,  [FromBody] CreateOwnerDto updateOwner)
        {
            if (updateOwner == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(OwnerID))
                return NotFound();

            var ownerMap = _mapper.Map<Owner>(updateOwner);
            ownerMap.ID = OwnerID;
            ownerMap.Country = _countryRepositry.GetCountry(CountryID);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Updating Your Entry");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
