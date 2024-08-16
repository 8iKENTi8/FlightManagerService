using FlightManagerService.Data;
using FlightManagerService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightManagerService.Services.Implementations
{
    public class FlightIdProvider : IFlightIdProvider
    {
        private readonly AppDbContext _context;

        public FlightIdProvider(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<int>> GetAllFlightIdsAsync()
        {
            return new HashSet<int>(await _context.Flights.Select(f => f.FlightId).ToListAsync());
        }
    }
}
