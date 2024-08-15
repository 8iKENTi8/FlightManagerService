using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightManagerService.Data;
using FlightManagerService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlightsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            return await _context.Flights.ToListAsync();
        }

        // POST: api/Flights/replace
        [HttpPost("replace")]
        public async Task<ActionResult> ReplaceFlights([FromBody] IEnumerable<Flight> flights)
        {
            _context.Flights.RemoveRange(_context.Flights);
            await _context.SaveChangesAsync();

            _context.Flights.AddRange(flights);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlights), new { }, null);
        }
        // POST: api/Flights/add
        [HttpPost("add")]
        public async Task<IActionResult> AddFlights([FromBody] IEnumerable<Flight> flights)
        {
            var existingFlightIds = _context.Flights.Select(f => f.FlightId).ToHashSet();
            var newFlights = flights.Where(f => !existingFlightIds.Contains(f.FlightId)).ToList();
            var existingFlights = flights.Where(f => existingFlightIds.Contains(f.FlightId)).ToList();

            if (newFlights.Any())
            {
                _context.Flights.AddRange(newFlights);
                await _context.SaveChangesAsync();
            }

            if (existingFlights.Any())
            {
                return Conflict(new
                {
                    Message = "Некоторые рейсы уже существуют в базе данных.",
                    ExistingFlights = existingFlights
                });
            }

            return Ok(); // Возвращаем Ok, если добавлены новые данные или если ничего не было добавлено
        }

    }
}
