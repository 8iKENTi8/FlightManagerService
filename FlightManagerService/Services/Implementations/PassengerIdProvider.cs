using FlightManagerService.Data;
using FlightManagerService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightManagerService.Services.Implementations
{
    public class PassengerIdProvider : IPassengerIdProvider
    {
        private readonly AppDbContext _context;

        public PassengerIdProvider(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<int>> GetAllPassengerIdsAsync()
        {
            return new HashSet<int>(await _context.Passengers.Select(p => p.PassengerId).ToListAsync());
        }
    }
}
