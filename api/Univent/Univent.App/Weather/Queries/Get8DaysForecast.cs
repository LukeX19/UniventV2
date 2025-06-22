using MediatR;
using Univent.App.Interfaces;
using Univent.App.Weather.Dtos;

namespace Univent.App.Weather.Queries
{
    public record Get8DaysForecastQuery() : IRequest<ICollection<DailyWeatherForecastResponseDto>>;

    public class Get8DaysForecastHandler : IRequestHandler<Get8DaysForecastQuery, ICollection<DailyWeatherForecastResponseDto>>
    {
        private readonly IWeatherService _weatherService;

        public Get8DaysForecastHandler(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<ICollection<DailyWeatherForecastResponseDto>> Handle(Get8DaysForecastQuery request, CancellationToken ct)
        {
            // Timișoara, Romania
            return await _weatherService.Get8DaysForecastAsync(45.7559, 21.2298, ct);
        }
    }
}
