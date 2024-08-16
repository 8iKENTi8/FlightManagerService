using FlightManagerService.Data;
using FlightManagerService.Models;
using Microsoft.EntityFrameworkCore;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .Include(t => t.Passenger)
            .Include(t => t.Flight)
            .ToListAsync();
    }

    public async Task<bool> ReplaceAllAsync(IEnumerable<Ticket> tickets)
    {
        // Удаляем все текущие билеты
        _context.Tickets.RemoveRange(_context.Tickets);
        await _context.SaveChangesAsync();

        // Добавляем новые билеты
        _context.Tickets.AddRange(tickets);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddAsync(IEnumerable<Ticket> tickets)
    {
        // Выполняем асинхронный запрос для получения идентификаторов рейсов и пассажиров
        var flightIds = await _context.Flights.Select(f => f.FlightId).ToListAsync();
        var passengerIds = await _context.Passengers.Select(p => p.PassengerId).ToListAsync();

        var flightIdSet = new HashSet<int>(flightIds);
        var passengerIdSet = new HashSet<int>(passengerIds);

        var invalidTickets = tickets
            .Where(t => !flightIdSet.Contains(t.FlightId) || !passengerIdSet.Contains(t.PassengerId))
            .ToList();

        var validTickets = tickets
            .Where(t => flightIdSet.Contains(t.FlightId) && passengerIdSet.Contains(t.PassengerId))
            .ToList();

        if (validTickets.Any())
        {
            _context.Tickets.AddRange(validTickets);
            await _context.SaveChangesAsync();
        }

        return !invalidTickets.Any(); // Возвращаем false, если есть недопустимые билеты
    }

    public async Task<Ticket?> GetByIdAsync(int ticketId)
    {
        return await _context.Tickets
            .Include(t => t.Passenger)
            .Include(t => t.Flight)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }

    public async Task<bool> TicketExistsAsync(int ticketId)
    {
        return await _context.Tickets.AnyAsync(t => t.TicketId == ticketId);
    }
}
