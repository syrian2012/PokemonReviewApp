﻿using AutoMapper;
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
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
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
            if(_reviewRepository.ReviewExists(reviewID))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewID));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(review);
        }
    }
}
