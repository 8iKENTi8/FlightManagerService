using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightManagerService.Data;
using FlightManagerService.Models;
using System.Collections.Generic;
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

        // POST: api/Flights
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> PostFlights([FromBody] IEnumerable<Flight> flights)
        {
            // Очистка таблицы перед добавлением новых данных
            _context.Flights.RemoveRange(_context.Flights);
            await _context.SaveChangesAsync();

            // Добавляем все рейсы в базу данных
            _context.Flights.AddRange(flights);
            await _context.SaveChangesAsync();

            // Возвращаем статус Created
            return CreatedAtAction(nameof(GetFlights), new { }, null);
        }

    }
}
