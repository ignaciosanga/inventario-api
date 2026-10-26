using System.Text.Json.Serialization;
using InventarioService.Data;
using InventarioService.Middleware;
using InventarioService.Repositories;
using InventarioService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'Default'.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();

builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    for (var intento = 1; intento <= 10; intento++)
    {
        try
        {
            await db.Database.EnsureCreatedAsync();
            logger.LogInformation("Base de datos lista.");
            break;
        }
        catch (Exception ex) when (intento < 10)
        {
            logger.LogWarning("Base de datos no disponible (intento {Intento}/10): {Mensaje}", intento, ex.Message);
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/health/db", async (AppDbContext db) =>
{
    var conectado = await db.Database.CanConnectAsync();
    return conectado
        ? Results.Ok(new { status = "ok", database = "connected" })
        : Results.Json(new { status = "error", database = "unreachable" }, statusCode: StatusCodes.Status503ServiceUnavailable);
});

app.MapControllers();

app.Run();
