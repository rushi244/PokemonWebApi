using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;
using PokemonWebApi.Repository;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryInterface _countryInterface;
        private readonly IMapper _mapper;
        public CountryController(ICountryInterface countryInterface,IMapper mapper)
        {
            _countryInterface = countryInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var Countries = _mapper.Map<List<CountryDto>>(_countryInterface.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int countryId)
        {
            if (!_countryInterface.IsCountryExists(countryId))
                return NotFound();
            var Country = _mapper.Map<CountryDto>(_countryInterface.GetCountry(countryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Country);
        }
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAOwner(int ownerId)
        {
            var Country = _mapper.Map<CountryDto>(_countryInterface.GetCountry(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Country);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CountryDto countrycreate)
        {
            if (countrycreate == null)
                return BadRequest(ModelState);
            var country = _countryInterface.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countrycreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country is Already Exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var countryMap = _mapper.Map<Country>(countrycreate);

            if (!_countryInterface.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");

        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryInterface.IsCountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_countryInterface.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryInterface.IsCountryExists(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryInterface.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryInterface.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
