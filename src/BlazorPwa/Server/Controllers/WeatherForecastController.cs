using BlazorPwa.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPwa.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get() => Store.Items;

        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast model)
        {
            Store.Items.Add(model);
            return Ok();
        }

        static class Store
        {
            public static List<WeatherForecast> Items { get; } = new();
        }
    }
}