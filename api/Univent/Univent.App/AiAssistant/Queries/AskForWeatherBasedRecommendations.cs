using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.AiAssistant.Queries
{
    public record AskForWeatherBasedRecommendationsQuery(Guid UserId) : IRequest<string>;

    public class AskForWeatherBasedRecommendationsHandler : IRequestHandler<AskForWeatherBasedRecommendationsQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWeatherService _weatherService;
        private readonly IAiAssistantService _aiAssistantService;

        public AskForWeatherBasedRecommendationsHandler(IUnitOfWork unitOfWork, IWeatherService weatherService, IAiAssistantService aiAssistantService)
        {
            _unitOfWork = unitOfWork;
            _weatherService = weatherService;
            _aiAssistantService = aiAssistantService;
        }

        public async Task<string> Handle(AskForWeatherBasedRecommendationsQuery request, CancellationToken ct)
        {
            var events = await _unitOfWork.EventRepository.GetAllPotentialEventsAsync(request.UserId, ct);
            if (!events.Any())
            {
                return "There are no upcoming events to suggest.";
            }

            var eventSummaries = events
                .Select(e => $"- Name: {e.Name}, Type: {e.Type?.Name}, Description: {e.Description}, Starts at: {e.StartTime:dddd, MMM dd}, Location: {e.LocationAddress}")
                .ToList();

            // Timișoara, Romania
            var forecast = await _weatherService.Get8DaysForecastAsync(45.7559, 21.2298, ct);

            return await _aiAssistantService.AskForWeatherBasedSuggestionsAsync(eventSummaries, forecast);
        }
    }
}
