using FlightManagerService.Models;
using FlightManagerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IRepository<Flight> _flightRepository;

        public FlightsController(IRepository<Flight> flightRepository)
        {
            _flightRepository = flightRepository;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            var flights = await _flightRepository.GetAllAsync();
            return Ok(flights);
        }

        // POST: api/Flights/replace
        [HttpPost("replace")]
        public async Task<IActionResult> ReplaceFlights([FromBody] IEnumerable<Flight> flights)
        {
            var success = await _flightRepository.ReplaceAllAsync(flights);
            if (success)
            {
                return CreatedAtAction(nameof(GetFlights), null);
            }
            return BadRequest("Failed to replace flights.");
        }

        // POST: api/Flights/add
        [HttpPost("add")]
        public async Task<IActionResult> AddFlights([FromBody] IEnumerable<Flight> flights)
        {
            var existingFlightIds = (await _flightRepository.GetAllAsync()).Select(f => f.FlightId).ToHashSet();
            var newFlights = flights.Where(f => !existingFlightIds.Contains(f.FlightId)).ToList();
            var existingFlights = flights.Where(f => existingFlightIds.Contains(f.FlightId)).ToList();

            if (newFlights.Any())
            {
                await _flightRepository.AddAsync(newFlights);
            }

            if (existingFlights.Any())
            {
                return Conflict(new
                {
                    Message = "Некоторые рейсы уже существуют в базе данных.",
                    ExistingFlights = existingFlights
                });
            }

            return Ok();
        }
    }
}
