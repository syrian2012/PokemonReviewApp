using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,IPokemonRepository pokemonRepository , IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            return Ok(reviews);
        }

        [HttpGet("{reviewID}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewID)
        {
            if(!_reviewRepository.ReviewExists(reviewID))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("{pokeID}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokeID)
        {
            if (!_pokemonRepository.PokemonExists(pokeID))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(reviews);
        }
        
    }
}
