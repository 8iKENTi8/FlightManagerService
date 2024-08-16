using FlightManagerService.Data;
using FlightManagerService.Models;
using Microsoft.EntityFrameworkCore; // Для методов расширения EF Core, включая ToListAsync


public class FlightRepository : IFlightRepository
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

    public async Task<bool> ReplaceAllAsync(IEnumerable<Flight> flights)
    {
        _context.Flights.RemoveRange(_context.Flights);
        await _context.SaveChangesAsync();

        _context.Flights.AddRange(flights);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddAsync(IEnumerable<Flight> flights)
    {
        var existingFlightIds = _context.Flights.Select(f => f.FlightId).ToHashSet();
        var newFlights = flights.Where(f => !existingFlightIds.Contains(f.FlightId)).ToList();
        var existingFlights = flights.Where(f => existingFlightIds.Contains(f.FlightId)).ToList();

        if (newFlights.Any())
        {
            _context.Flights.AddRange(newFlights);
            await _context.SaveChangesAsync();
        }

        return !existingFlights.Any(); // Возвращаем false, если есть конфликтующие рейсы
    }
}