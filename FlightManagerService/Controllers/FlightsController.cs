using FlightManagerService.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly IFlightRepository _flightRepository;

    public FlightsController(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
    {
        var flights = await _flightRepository.GetAllAsync();
        return Ok(flights);
    }

    [HttpPost("replace")]
    public async Task<IActionResult> ReplaceFlights([FromBody] IEnumerable<Flight> flights)
    {
        var result = await _flightRepository.ReplaceAllAsync(flights);
        if (result)
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddFlights([FromBody] IEnumerable<Flight> flights)
    {
        var result = await _flightRepository.AddAsync(flights);
        if (result)
        {
            return Ok();
        }
        return Conflict(new { Message = "Некоторые рейсы уже существуют в базе данных." });
    }
}
