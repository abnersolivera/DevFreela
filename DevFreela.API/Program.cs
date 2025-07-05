using DevFreela.API.ExceptionHandler;
using DevFreela.API.Models;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FreelanceTotalCostConfiguration>(
    builder.Configuration.GetSection("FreelanceTotalCostConfiguration")
    );
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddDbContext<DevFreelaDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseAuthorization();
app.MapControllers();
app.Run();