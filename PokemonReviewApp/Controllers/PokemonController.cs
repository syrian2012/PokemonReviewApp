using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository , IMapper mapper)
        {
            _pokemonRepository= pokemonRepository;
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





    }
}
