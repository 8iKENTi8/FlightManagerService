using Microsoft.EntityFrameworkCore;
using FlightManagerService.Data;

using FlightManagerService.Models;

var builder = WebApplication.CreateBuilder(args);

// ��������� DbContext ��� ������ � ����� ������ SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ����������� ��� �������� Flight
builder.Services.AddScoped<IFlightRepository, FlightRepository>();


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
