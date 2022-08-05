using BlazorPwa.Client;
using BlazorPwa.Client.Data;
using DnetIndexedDb;
using DnetIndexedDb.Fluent;
using DnetIndexedDb.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<WeatherForecastService>();

builder.Services.AddIndexedDbDatabase<WeatherForecastLocalContext>(options =>
{
    var indexedDbDatabaseModel = new IndexedDbDatabaseModel()
        .WithName(nameof(WeatherForecastLocalContext))
        .WithVersion(1);

    indexedDbDatabaseModel.AddStore<WeatherForecastLocalModel>();

    options.UseDatabase(indexedDbDatabaseModel);
});

builder.Services.AddSingleton<ConnectionState>();

await builder.Build().RunAsync();
