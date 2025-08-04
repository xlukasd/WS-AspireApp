using Microsoft.EntityFrameworkCore;

namespace WS_AspireApp.ApiService.Endpoints;

internal static class WeatherForecastEndpoints
{
    internal static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast", async (WeatherForecastDbContext db) =>
        {
            var connectionString = db.Database.GetConnectionString();
            var canConnect = await db.Database.CanConnectAsync();

            try
            {
                var forecasts = await db.WeatherForecasts.AsNoTracking().ToListAsync();
                return forecasts;
            }
            catch (Exception)
            {
                return new List<WeatherForecast> { new WeatherForecast(DateTime.UtcNow, 10, $"{connectionString} --- {canConnect}") };
            }
        })
        .WithName("GetWeatherForecast");
    }
}
