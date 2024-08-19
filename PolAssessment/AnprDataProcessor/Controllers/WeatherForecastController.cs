using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Models;

namespace AnprDataProcessor.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger, AnprDataDbContext anprDataDbContext) 
        : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger = logger;
    private readonly AnprDataDbContext _anprDataDbContext = anprDataDbContext;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        //var anpr = new AnprRecord
        //{
        //    LicensePlate = "ABC-123",
        //    Longitude = 52.0,
        //    Latitude = 4.0,
        //    ExactDateTime = DateTime.Now
        //};
        //_anprDataDbContext.AnprRecords.Add(anpr);
        //_anprDataDbContext.SaveChanges();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
