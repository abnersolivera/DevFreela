using DevFreela.API.ExceptionHandler;
using DevFreela.Application;
using DevFreela.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

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