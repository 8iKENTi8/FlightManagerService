using FlightManagerService.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
// Контроллер для работы с данными рейсов.
public class FlightsController : ControllerBase
{
    private readonly IFlightRepository _flightRepository;

    // Конструктор контроллера, принимающий зависимость в виде репозитория рейсов через Dependency Injection (DI).
    public FlightsController(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    // HTTP GET метод для получения всех рейсов.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
    {
        // Получение всех рейсов из базы данных через репозиторий.
        var flights = await _flightRepository.GetAllAsync();

        // Возвращение списка рейсов с HTTP-статусом 200 (OK).
        return Ok(flights);
    }

    // HTTP POST метод для полной замены всех рейсов в базе данных.
    [HttpPost("replace")]
    public async Task<IActionResult> ReplaceFlights([FromBody] IEnumerable<Flight> flights)
    {
        // Замена всех существующих записей рейсов в базе данных на новые.
        var result = await _flightRepository.ReplaceAllAsync(flights);

        // Если замена прошла успешно, возвращается HTTP-статус 204 (No Content).
        if (result)
        {
            return NoContent();
        }

        return BadRequest();
    }

    // HTTP POST метод для добавления новых рейсов в базу данных.
    [HttpPost("add")]
    public async Task<IActionResult> AddFlights([FromBody] IEnumerable<Flight> flights)
    {
        // Добавление новых рейсов в базу данных через репозиторий.
        var result = await _flightRepository.AddAsync(flights);

        // Если добавление прошло успешно, возвращается HTTP-статус 200 (OK).
        if (result)
        {
            return Ok();
        }

        return Conflict(new { Message = "Некоторые рейсы уже существуют в базе данных." });
    }
}
