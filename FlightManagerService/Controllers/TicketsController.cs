using FlightManagerService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IPassengerRepository _passengerRepository;

    public TicketsController(ITicketRepository ticketRepository, IFlightRepository flightRepository, IPassengerRepository passengerRepository)
    {
        _ticketRepository = ticketRepository;
        _flightRepository = flightRepository;
        _passengerRepository = passengerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return Ok(tickets);
    }

    [HttpPost("replace")]
    public async Task<IActionResult> ReplaceTickets([FromBody] IEnumerable<Ticket> tickets)
    {
        var result = await _ticketRepository.ReplaceAllAsync(tickets);
        if (result)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTickets([FromBody] IEnumerable<Ticket> tickets)
    {
        var result = await _ticketRepository.AddAsync(tickets);
        if (result)
        {
            return Ok();
        }
        return Conflict(new { Message = "Некоторые билеты содержат несуществующие рейсы или пассажиров." });
    }
}
