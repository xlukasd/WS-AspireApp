using Microsoft.EntityFrameworkCore;

namespace WS_AspireApp.ApiService;

public class WeatherForecastDbContext : DbContext
{
    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
        : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().ToTable("WeatherForecast");
        modelBuilder.Entity<WeatherForecast>().HasNoKey();
        modelBuilder.Entity<WeatherForecast>().Property(wf => wf.Date).HasColumnType("datetime");
        modelBuilder.Entity<WeatherForecast>().Property(wf => wf.TemperatureC).HasColumnType("int");
        modelBuilder.Entity<WeatherForecast>().Property(wf => wf.Summary).HasColumnType("nvarchar(50)");
    }
}
