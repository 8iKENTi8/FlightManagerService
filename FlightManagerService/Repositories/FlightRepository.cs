using FlightManagerService.Data;
using FlightManagerService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagerService.Repositories
{
    public class FlightRepository : IRepository<Flight>
    {
        private readonly AppDbContext _context;

        public FlightRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<bool> ReplaceAllAsync(IEnumerable<Flight> entities)
        {
            _context.Flights.RemoveRange(_context.Flights);
            await _context.SaveChangesAsync();

            _context.Flights.AddRange(entities);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task AddAsync(IEnumerable<Flight> entities)
        {
            _context.Flights.AddRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
