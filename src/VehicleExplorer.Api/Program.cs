using System.Net.Http.Headers;
using VehicleExplorer.Api.Services;
using VehicleExplorer.Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHttpClient<IVehicleService, VehicleService>(
    (serviceProvider, client) =>
    {
        var configuration =
            serviceProvider.GetRequiredService<IConfiguration>();

        var baseUrl =
            configuration["NhtsaApi:BaseUrl"]
            ?? throw new InvalidOperationException(
                "NHTSA API base URL is not configured.");

        client.BaseAddress = new Uri(baseUrl);

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        client.Timeout = TimeSpan.FromSeconds(15);
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//for the UI
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
