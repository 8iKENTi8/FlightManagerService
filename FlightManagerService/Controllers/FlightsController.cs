using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightManagerService.Data;
using FlightManagerService.Models;

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

        // POST: api/Flights
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            // Возвращаем статус Created с данными созданного рейса
            return CreatedAtAction(nameof(GetFlights), new { id = flight.FlightId }, flight);
        }
    }
}
