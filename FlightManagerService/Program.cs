using Microsoft.EntityFrameworkCore;
using FlightManagerService.Data;
using FlightManagerService.Repositories;
using FlightManagerService.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем DbContext для работы с базой данных SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозитория для сущности Flight
builder.Services.AddScoped<IRepository<Flight>, FlightRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
