using FlightManagerService.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
// Контроллер для работы с пассажирами. 
public class PassengersController : ControllerBase
{
    private readonly IPassengerRepository _passengerRepository;

    // Конструктор контроллера, принимающий зависимость в виде репозитория пассажиров.
    public PassengersController(IPassengerRepository passengerRepository)
    {
        _passengerRepository = passengerRepository;
    }

    // Метод для получения всех пассажиров.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Passenger>>> GetPassengers()
    {
        var passengers = await _passengerRepository.GetAllAsync();
        return Ok(passengers); // Возвращает список пассажиров с HTTP-статусом 200 (OK).
    }

    // Метод для замены всех пассажиров в базе данных.
    [HttpPost("replace")]
    public async Task<IActionResult> ReplacePassengers([FromBody] IEnumerable<Passenger> passengers)
    {
        var result = await _passengerRepository.ReplaceAllAsync(passengers);
        if (result)
        {
            return NoContent(); // Возвращает HTTP-статус 204 (No Content), если операция успешна.
        }
        return BadRequest(); // Возвращает HTTP-статус 400 (Bad Request), если операция не удалась.
    }

    // Метод для добавления новых пассажиров в базу данных.
    [HttpPost("add")]
    public async Task<IActionResult> AddPassengers([FromBody] IEnumerable<Passenger> passengers)
    {
        var result = await _passengerRepository.AddAsync(passengers);
        if (result)
        {
            return Ok(); // Возвращает HTTP-статус 200 (OK), если операция успешна.
        }
        return Conflict(new { Message = "Некоторые пассажиры уже существуют в базе данных." });
    }
}