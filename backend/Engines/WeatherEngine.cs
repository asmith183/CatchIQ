using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CatchIQ.API.Engines.Interfaces;
using CatchIQ.API.Models;
using CatchIQ.API.Models.Enums;

namespace CatchIQ.API.Engines;
public class WeatherEngine(HttpClient httpClient, ILogger<WeatherEngine> logger) : IWeatherEngine
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<WeatherEngine> _logger = logger;

    // Open-Meteo's forecast endpoint only reaches 92 days into the past;
    // older catches have to be looked up through the archive endpoint.
    private const int ForecastPastDaysLimit = 92;

    public async Task<WeatherSnapshot?> GetSnapshotAsync(double latitude, double longitude, DateTimeOffset caughtAt)
    {
        var utc = caughtAt.UtcDateTime;
        var host = (DateTime.UtcNow - utc).TotalDays > ForecastPastDaysLimit
            ? "https://archive-api.open-meteo.com/v1/archive"
            : "https://api.open-meteo.com/v1/forecast";

        var date = utc.ToString("yyyy-MM-dd");
        var url = host
            + "?latitude=" + latitude.ToString(CultureInfo.InvariantCulture)
            + "&longitude=" + longitude.ToString(CultureInfo.InvariantCulture)
            + $"&start_date={date}&end_date={date}"
            + "&hourly=temperature_2m,wind_speed_10m,wind_direction_10m,pressure_msl,weather_code"
            + "&temperature_unit=fahrenheit&wind_speed_unit=mph&timezone=UTC";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);
            var hourly = response?.Hourly;

            if (hourly == null || hourly.Time.Count == 0)
                return null;

            // The response holds one UTC day of hourly rows, so the row index
            // is the hour of day, rounded to the nearest hour.
            var index = utc.Minute >= 30 ? utc.Hour + 1 : utc.Hour;
            index = Math.Min(index, hourly.Time.Count - 1);

            return new WeatherSnapshot(
                AirTempF: hourly.TemperatureF.ElementAtOrDefault(index),
                WindSpeedMph: hourly.WindSpeedMph.ElementAtOrDefault(index),
                WindDirection: ToWindDirection(hourly.WindDirectionDeg.ElementAtOrDefault(index)),
                PressureMb: hourly.PressureMb.ElementAtOrDefault(index),
                SkyCondition: ToSkyCondition(hourly.WeatherCode.ElementAtOrDefault(index))
            );
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "Weather lookup failed for ({Latitude}, {Longitude}) at {CaughtAt}", latitude, longitude, caughtAt);
            return null;
        }
    }

    #region Helper Methods
    private static WindDirection? ToWindDirection(double? degrees)
    {
        if (degrees == null)
            return null;

        return (WindDirection)((int)Math.Round(degrees.Value / 45.0) % 8);
    }

    private static SkyCondition? ToSkyCondition(int? weatherCode)
    {
        // WMO weather codes: 0-2 clear to partly cloudy, 3/45/48 overcast or
        // fog, 95+ thunderstorms, everything between is some form of precipitation.
        return weatherCode switch
        {
            null => null,
            0 or 1 => SkyCondition.Clear,
            2 => SkyCondition.PartlyCloudy,
            3 or 45 or 48 => SkyCondition.Overcast,
            >= 95 => SkyCondition.Stormy,
            _ => SkyCondition.Rainy
        };
    }
    #endregion

    #region Response Models
    private sealed class OpenMeteoResponse
    {
        [JsonPropertyName("hourly")]
        public HourlyBlock? Hourly { get; set; }
    }

    private sealed class HourlyBlock
    {
        [JsonPropertyName("time")]
        public List<string> Time { get; set; } = [];

        [JsonPropertyName("temperature_2m")]
        public List<double?> TemperatureF { get; set; } = [];

        [JsonPropertyName("wind_speed_10m")]
        public List<double?> WindSpeedMph { get; set; } = [];

        [JsonPropertyName("wind_direction_10m")]
        public List<double?> WindDirectionDeg { get; set; } = [];

        [JsonPropertyName("pressure_msl")]
        public List<double?> PressureMb { get; set; } = [];

        [JsonPropertyName("weather_code")]
        public List<int?> WeatherCode { get; set; } = [];
    }
    #endregion
}
