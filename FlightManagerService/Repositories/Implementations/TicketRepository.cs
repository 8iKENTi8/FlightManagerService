using FlightManagerService.Models;
using FlightManagerService.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets.Include(t => t.Passenger).Include(t => t.Flight).ToListAsync();
    }

    public async Task<bool> ReplaceAllAsync(IEnumerable<Ticket> tickets)
    {
        _context.Tickets.RemoveRange(_context.Tickets);
        await _context.SaveChangesAsync();

        _context.Tickets.AddRange(tickets);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddAsync(IEnumerable<Ticket> tickets)
    {
        var flightIds = _context.Flights.Select(f => f.FlightId).ToHashSet();
        var passengerIds = _context.Passengers.Select(p => p.PassengerId).ToHashSet();

        var invalidTickets = tickets.Where(t => !flightIds.Contains(t.FlightId) || !passengerIds.Contains(t.PassengerId)).ToList();
        var validTickets = tickets.Where(t => flightIds.Contains(t.FlightId) && passengerIds.Contains(t.PassengerId)).ToList();

        if (validTickets.Any())
        {
            _context.Tickets.AddRange(validTickets);
            await _context.SaveChangesAsync();
        }

        return !invalidTickets.Any(); // Возвращаем false, если есть недопустимые билеты
    }


    public async Task<Ticket?> GetByIdAsync(int ticketId)
    {
        return await _context.Tickets.Include(t => t.Passenger).Include(t => t.Flight)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }


    public async Task<bool> TicketExistsAsync(int ticketId)
    {
        return await _context.Tickets.AnyAsync(t => t.TicketId == ticketId);
    }
}
