using BlazorPwa.Client.Data;
using BlazorPwa.Shared;
using System.Net.Http.Json;

namespace BlazorPwa.Client;

public class WeatherForecastService
{
    private readonly ConnectionState _connectionState;

    public HttpClient Http { get; }
    public WeatherForecastLocalContext Context { get; }

    public WeatherForecastService(HttpClient http, WeatherForecastLocalContext context, ConnectionState connectionState)
    {
        Http = http ?? throw new ArgumentNullException(nameof(http));
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _connectionState = connectionState;
    }

    public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecastsAsync()
    {
        await Context.OpenIndexedDb();

        var result = (await Context.GetAll<WeatherForecastLocalModel>())
            .Select(w => new WeatherForecast
            {
                Date = w.Date,
                Summary = w.Summary,
                TemperatureC = w.TemperatureC
            }).ToList();

        var itemsFromServer = await LoadFromServer();
        if (itemsFromServer.Any())
        {
            result.AddRange(itemsFromServer);
        }

        return result.OrderBy(i => i.Date);

    }

    private async Task<IEnumerable<WeatherForecast>> LoadFromServer()
    {
        if (!_connectionState.IsOnline)
        {
            return Enumerable.Empty<WeatherForecast>();
        }

        try
        {
            var model = await Http.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast");
            return model ?? Enumerable.Empty<WeatherForecast>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<WeatherForecast>();
        }
    }

    public async Task SaveWeatherForecastAsync(WeatherForecast model)
    {
        await Context.OpenIndexedDb();

        try
        {
            await Context.AddItems(new List<WeatherForecastLocalModel>
            {
                new WeatherForecastLocalModel { Date = model.Date, Summary = model.Summary, TemperatureC = model.TemperatureC }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        if (_connectionState.IsOnline)
        {
            await Http.PostAsJsonAsync("WeatherForecast", model);
        }
    }

    public async Task Synchronize()
    {
        if (!_connectionState.IsOnline)
        {
            return;
        }

        var result = (await Context.GetAll<WeatherForecastLocalModel>())
            .Select(w => new WeatherForecast
            {
                Date = w.Date,
                Summary = w.Summary,
                TemperatureC = w.TemperatureC
            }).ToArray();

        foreach (var item in result)
        {
            await Http.PostAsJsonAsync("WeatherForecast", item);
        }
    }
}
