using Microsoft.EntityFrameworkCore;
using FlightManagerService.Data;
using FlightManagerService.Models;
using FlightManagerService.Services.Implementations;
using FlightManagerService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ��������� DbContext ��� ������ � ����� ������ SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ������������ ��� ���������
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// ����������� �����������
builder.Services.AddScoped<IFlightIdProvider, FlightIdProvider>();
builder.Services.AddScoped<IPassengerIdProvider, PassengerIdProvider>();

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
