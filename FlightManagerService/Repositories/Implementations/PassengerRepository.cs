using FlightManagerService.Data;
using FlightManagerService.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class PassengerRepository : IPassengerRepository
{
    private readonly AppDbContext _context;

    public PassengerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Passenger>> GetAllAsync()
    {
        return await _context.Passengers.ToListAsync();
    }

    public async Task<bool> ReplaceAllAsync(IEnumerable<Passenger> passengers)
    {
        _context.Passengers.RemoveRange(_context.Passengers);
        await _context.SaveChangesAsync();

        _context.Passengers.AddRange(passengers);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddAsync(IEnumerable<Passenger> passengers)
    {
        var existingPassengerIds = _context.Passengers.Select(p => p.PassengerId).ToHashSet();
        var newPassengers = passengers.Where(p => !existingPassengerIds.Contains(p.PassengerId)).ToList();
        var existingPassengers = passengers.Where(p => existingPassengerIds.Contains(p.PassengerId)).ToList();

        if (newPassengers.Any())
        {
            _context.Passengers.AddRange(newPassengers);
            await _context.SaveChangesAsync();
        }

        return !existingPassengers.Any(); // Возвращаем false, если есть конфликтующие записи
    }
}
