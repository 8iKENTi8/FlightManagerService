using FlightManagerService.Models;
using FlightManagerService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IFlightIdProvider _flightIdProvider; // Инжектируем провайдеры
    private readonly IPassengerIdProvider _passengerIdProvider;

    public TicketsController(ITicketRepository ticketRepository, IFlightIdProvider flightIdProvider, IPassengerIdProvider passengerIdProvider)
    {
        _ticketRepository = ticketRepository;
        _flightIdProvider = flightIdProvider;
        _passengerIdProvider = passengerIdProvider;
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
        // Получаем все существующие рейсы и пассажиров
        var flightIds = await _flightIdProvider.GetAllFlightIdsAsync();
        var passengerIds = await _passengerIdProvider.GetAllPassengerIdsAsync();

        var invalidTickets = tickets
            .Where(t => !flightIds.Contains(t.FlightId) || !passengerIds.Contains(t.PassengerId))
            .ToList();

        if (invalidTickets.Any())
        {
            return BadRequest(new { Message = "Некоторые билеты содержат несуществующие рейсы или пассажиров." });
        }

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
        var flightIds = await _flightIdProvider.GetAllFlightIdsAsync();
        var passengerIds = await _passengerIdProvider.GetAllPassengerIdsAsync();

        var invalidTickets = tickets
            .Where(t => !flightIds.Contains(t.FlightId) || !passengerIds.Contains(t.PassengerId))
            .ToList();

        if (invalidTickets.Any())
        {
            return BadRequest(new { Message = "Некоторые билеты содержат несуществующие рейсы или пассажиров." });
        }

        var result = await _ticketRepository.AddAsync(tickets);
        if (result)
        {
            return Ok();
        }
        return Conflict(new { Message = "Некоторые билеты уже существуют." });
    }
}
