using FlightManagerService.Models;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<Ticket?> GetByIdAsync(int ticketId); // Обозначаем возможность возврата null
    Task<bool> TicketExistsAsync(int ticketId);
}
