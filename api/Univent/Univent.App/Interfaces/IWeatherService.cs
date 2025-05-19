using Univent.App.Weather.Dtos;

namespace Univent.App.Interfaces
{
    public interface IWeatherService
    {
        Task<ICollection<DailyWeatherForecastResponseDto>> Get8DaysForecastAsync(double lat, double lon, CancellationToken ct = default);
    }
}
