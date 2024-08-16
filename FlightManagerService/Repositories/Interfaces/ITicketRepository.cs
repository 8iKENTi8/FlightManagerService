using FlightManagerService.Models;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<Ticket?> GetByIdAsync(int ticketId);
    Task<bool> TicketExistsAsync(int ticketId);
}
