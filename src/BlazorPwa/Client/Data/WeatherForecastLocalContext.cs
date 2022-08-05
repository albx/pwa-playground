using DnetIndexedDb;
using Microsoft.JSInterop;

namespace BlazorPwa.Client.Data;

public class WeatherForecastLocalContext : IndexedDbInterop
{
    public WeatherForecastLocalContext(
        IJSRuntime jsRuntime,
        IndexedDbOptions<WeatherForecastLocalContext> indexedDbDatabaseOptions) : base(jsRuntime, indexedDbDatabaseOptions)
    {
    }
}
