using DnetIndexedDb;

namespace BlazorPwa.Client.Data;

public class WeatherForecastLocalModel
{
    [IndexDbKey(AutoIncrement = true)]
    public int Id { get; set; }

    [IndexDbIndex]
    public DateTime Date { get; set; }

    public string? Summary { get; set; }

    public int TemperatureC { get; set; }
}
