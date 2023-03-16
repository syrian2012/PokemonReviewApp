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

    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository ,IReviewRepository reviewRepository, IMapper mapper)
        {
            _pokemonRepository= pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpGet("{pokeID}")]
        [ProducesResponseType(200,Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeID)
        {
            if (!_pokemonRepository.PokemonExists(pokeID))
                return NotFound();
            
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeID));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);            
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerID, [FromQuery] int categoryID, [FromBody] CreatePokemonDto createPokemon)
        {
            if (createPokemon == null)
                return BadRequest(ModelState);

            if(ownerID == 0)
                return BadRequest(ModelState);

            if(categoryID == 0)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(createPokemon);

            var pokemon = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToLower() == createPokemon.Name.Trim().ToLower()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!_pokemonRepository.CreatePokemon(ownerID,categoryID,pokemonMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Saving Your Entry");
                return StatusCode(500, ModelState);
            }

            return Ok("Pokemon Created Successfully");
        }

        [HttpPut("{PokeID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int PokeID,[FromBody] CreatePokemonDto updatePokemon)
        {
            if (updatePokemon == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(PokeID))
                return NotFound();

            var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);
            pokemonMap.ID = PokeID;
            
            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Updating Your Entry");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{PokemonID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int PokemonID)
        {
            if (!_pokemonRepository.PokemonExists(PokemonID))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviews = _reviewRepository.GetReviewsOfAPokemon(PokemonID).ToList();

            if (!_reviewRepository.DeleteReviews(reviews))
            {
                ModelState.AddModelError("", "Somthing Went Wrong While Deleting Pokemon's Reviews");
            }

            if (!_pokemonRepository.DeletePokemon(PokemonID))
            {
                ModelState.AddModelError("", "Somthing Went Wrong While Deleting");
            }
            return Ok("Deleted!!");
        }
    }
}
