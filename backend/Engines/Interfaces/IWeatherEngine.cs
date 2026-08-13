using CatchIQ.API.Models;

namespace CatchIQ.API.Engines.Interfaces;
public interface IWeatherEngine
{
    Task<WeatherSnapshot?> GetSnapshotAsync(double latitude, double longitude, DateTimeOffset caughtAt);
}
