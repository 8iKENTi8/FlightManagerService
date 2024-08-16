using FlightManagerService.Data;
using FlightManagerService.Models;
using FlightManagerService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PassengerRepository : IPassengerRepository
{
    private readonly AppDbContext _context;
    private readonly IPassengerIdProvider _passengerIdProvider;

    public PassengerRepository(AppDbContext context, IPassengerIdProvider passengerIdProvider)
    {
        _context = context;
        _passengerIdProvider = passengerIdProvider;
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
        var existingPassengerIds = await _passengerIdProvider.GetAllPassengerIdsAsync();
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
