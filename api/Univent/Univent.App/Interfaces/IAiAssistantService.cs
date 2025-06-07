using Univent.App.Weather.Dtos;

namespace Univent.App.Interfaces
{
    public interface IAiAssistantService
    {
        Task<string> AskForInterestsBasedSuggestionsAsync(string userDescription, ICollection<string> eventSummaries);
        Task<string> AskForLocationBasedSuggestionsAsync(string locationInfo, ICollection<string> eventSummaries);
        Task<string> AskForTimeBasedSuggestionsAsync(string timePreference, ICollection<string> eventSummaries);
        Task<string> AskForWeatherBasedSuggestionsAsync(ICollection<string> eventSummaries, ICollection<DailyWeatherForecastResponseDto> forecast);
    }
}
