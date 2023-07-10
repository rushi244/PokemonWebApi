using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Data;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;
using PokemonWebApi.Repository;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewInterface _reviewInterface;
        
        private readonly IPokemonInterface _pokeInterface;
        private readonly IReviewerInterface _revInterface;
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        public ReviewController(IReviewInterface reviewInterface, IPokemonInterface pokemonInterface,IReviewerInterface revInterface,IMapper mapper)
        {
            _reviewInterface = reviewInterface;
            _pokeInterface = pokemonInterface;
            _revInterface= revInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewInterface.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewInterface.IsReviewExists(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewInterface.GetReview(reviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var review = _mapper.Map<List<ReviewDto>>(_reviewInterface.GetReviewsofPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _reviewInterface.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = _pokeInterface.GetPokemon(pokeId);
            reviewMap.Reviewer = _revInterface.GetReviewer(reviewerId);
            if (!_reviewInterface.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewInterface.IsReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewInterface.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewInterface.IsReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewInterface.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewInterface.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

        // Added missing delete range of reviews by a reviewer **>CK
        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_revInterface.IsReviewerExists(reviewerId))
                return NotFound();

            var reviewsToDelete = _revInterface.GetReviewByReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewInterface.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
