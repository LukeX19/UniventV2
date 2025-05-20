using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.AiAssistant.Queries
{
    public record AskQuestionAboutTimePreferencesQuery(string TimePreference) : IRequest<string>;

    public class AskQuestionAboutTimePreferencesHandler : IRequestHandler<AskQuestionAboutTimePreferencesQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiAssistantService _aiAssistantService;

        public AskQuestionAboutTimePreferencesHandler(IUnitOfWork unitOfWork, IAiAssistantService aiAssistantService)
        {
            _unitOfWork = unitOfWork;
            _aiAssistantService = aiAssistantService;
        }

        public async Task<string> Handle(AskQuestionAboutTimePreferencesQuery request, CancellationToken ct)
        {
            var events = await _unitOfWork.EventRepository.GetAllUpcomingEventsAsync(ct);

            if (!events.Any())
            {
                return "There are no upcoming events to suggest.";
            }

            // Build summaries
            var summaries = events
                .Select(e =>$"- Name: {e.Name}, Starts at: {e.StartTime.ToString("f")}, Location: {e.LocationAddress}")
                .ToList();

            return await _aiAssistantService.AskForTimeBasedSuggestionsAsync(request.TimePreference, summaries);
        }
    }
}
