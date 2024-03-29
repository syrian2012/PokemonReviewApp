﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,
            IPokemonRepository pokemonRepository 
            ,IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int pokemonID, [FromQuery] int reviewerID, [FromBody] CreateReviewDto createReview)
        {
            if (createReview == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(createReview);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonID);

            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerID);


            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Saving Your Entry");
                return StatusCode(500, ModelState);
            }

            return Ok("Review Created Successfully");
        }

        [HttpPut("{ReviewID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ReviewID, [FromQuery] int reviewerID, [FromQuery] int pokiID, [FromBody] CreateReviewDto updateReview)
        {
            if (updateReview == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(ReviewID))
                return NotFound();

            var reviewMap = _mapper.Map<Review>(updateReview);
            reviewMap.ID = ReviewID;
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerID);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokiID);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Updating Your Entry");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{ReviewID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int ReviewID)
        {
            if (!_reviewRepository.ReviewExists(ReviewID))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = _reviewRepository.GetReview(ReviewID);

            if (!_reviewRepository.DeleteReview(review))
            {
                ModelState.AddModelError("", "Somthing Went Wrong While Deleting");
            }
            return Ok("Deleted!!");
        }
    }
}
