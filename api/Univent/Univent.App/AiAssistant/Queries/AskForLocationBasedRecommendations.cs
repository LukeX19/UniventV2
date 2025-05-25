using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.AiAssistant.Queries
{
    public record AskForLocationBasedRecommendationsQuery(Guid UserId, string LocationDescription) : IRequest<string>;

    public class AskForLocationBasedRecommendationsHandler : IRequestHandler<AskForLocationBasedRecommendationsQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiAssistantService _aiService;

        public AskForLocationBasedRecommendationsHandler(IUnitOfWork unitOfWork, IAiAssistantService aiService)
        {
            _unitOfWork = unitOfWork;
            _aiService = aiService;
        }

        public async Task<string> Handle(AskForLocationBasedRecommendationsQuery request, CancellationToken ct)
        {
            var events = await _unitOfWork.EventRepository.GetAllPotentialEventsAsync(request.UserId, ct);

            if (!events.Any())
            {
                return "There are no upcoming events to suggest.";
            }

            // Build summaries
            var summaries = events
                .Select(e =>$"- Name: {e.Name}, Type: {e.Type?.Name}, Description: {e.Description}, Location: {e.LocationAddress}, Starts at: {e.StartTime:g}")
                .ToList();

            return await _aiService.AskForLocationBasedSuggestionsAsync(request.LocationDescription, summaries);
        }
    }
}
