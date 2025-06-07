using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Univent.App.Interfaces;
using Univent.App.Weather.Dtos;

namespace Univent.Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ICollection<DailyWeatherForecastResponseDto>> Get8DaysForecastAsync(double lat, double lon, CancellationToken ct = default)
        {
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&exclude=current,minutely,hourly,alerts&units=metric&appid={apiKey}";

            var response = await _httpClient.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);
            var doc = JsonDocument.Parse(json);

            var forecasts = new List<DailyWeatherForecastResponseDto>();

            foreach (var day in doc.RootElement.GetProperty("daily").EnumerateArray())
            {
                forecasts.Add(new DailyWeatherForecastResponseDto
                {
                    Date = DateTimeOffset.FromUnixTimeSeconds(day.GetProperty("dt").GetInt64()).UtcDateTime.Date,
                    Condition = day.GetProperty("weather")[0].GetProperty("main").GetString(),
                    Description = day.GetProperty("weather")[0].GetProperty("description").GetString(),
                    TempMin = day.GetProperty("temp").GetProperty("min").GetDouble(),
                    TempMax = day.GetProperty("temp").GetProperty("max").GetDouble(),
                    PrecipitationProbability = day.TryGetProperty("pop", out var pop) ? pop.GetDouble() : 0,
                    Uvi = day.TryGetProperty("uvi", out var uvi) ? uvi.GetDouble() : 0,
                    Humidity = day.TryGetProperty("humidity", out var humidity) ? humidity.GetInt32() : 0,
                    WindSpeed = day.TryGetProperty("wind_speed", out var wind) ? wind.GetDouble() : 0,
                    RainVolume = day.TryGetProperty("rain", out var rain) ? rain.GetDouble() : null,
                    SnowVolume = day.TryGetProperty("snow", out var snow) ? snow.GetDouble() : null
                });
            }

            return forecasts;
        }
    }
}
