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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(reviewers);
        }

        [HttpGet("{ReviewerID}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int ReviewerID)
        {
            if (!_reviewerRepository.ReviewerExists(ReviewerID)) { return NotFound(); }

            var reviewer = _mapper.Map<Reviewer>(_reviewerRepository.GetReviewer(ReviewerID));

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(reviewer);
        }

        [HttpGet("{ReviewerID}/Reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewsByReviewer(int ReviewerID)
        {
            if (!_reviewerRepository.ReviewerExists(ReviewerID)) { return NotFound(); }

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(ReviewerID));

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] CreateReviewerDto createReviewer)
        {
            if (createReviewer == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(createReviewer);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Saving Your Entry");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviewer Created Successfully");
        }


    }
}
