using WS_AspireApp.ApiService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ErrorMiddleware>();

builder.AddSqlServerDbContext<WeatherForecastDbContext>(connectionName: "WeatherForecastDb");
builder.AddAzureBlobClient("blobs");

builder.Services.AddHttpClient<DummyServiceHttpClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://dummyservice");
});

var app = builder.Build();

app.UseMiddleware<ErrorMiddleware>();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

WeatherForecastEndpoints.AddEndpoints(app);
StoredFilesEndpoints.AddEndpoints(app);

app.MapDefaultEndpoints();

app.Run();