using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.AiAssistant.Queries
{
    public record AskQuestionAboutEventsQuery(string Question) : IRequest<string>;

    public class AskQuestionAboutEventsHandler : IRequestHandler<AskQuestionAboutEventsQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiAssistantService _aiAssistantService;

        public AskQuestionAboutEventsHandler(IUnitOfWork unitOfWork, IAiAssistantService aiAssistantService)
        {
            _unitOfWork = unitOfWork;
            _aiAssistantService = aiAssistantService;
        }

        public async Task<string> Handle(AskQuestionAboutEventsQuery request, CancellationToken ct)
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync(ct);

            if (!events.Any())
            {
                return "There are no events available at this moment.";
            }

            var eventsNames = events
               .Where(e => !string.IsNullOrWhiteSpace(e.Name))
               .Select(e => e.Name)
               .ToList();

            return await _aiAssistantService.AskQuestionAboutEventsAsync(request.Question, eventsNames);
        }
    }
}
