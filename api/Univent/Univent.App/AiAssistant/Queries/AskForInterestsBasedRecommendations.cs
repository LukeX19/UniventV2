﻿using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.AiAssistant.Queries
{
    public record AskForInterestsBasedRecommendationsQuery(Guid UserId, string UserDescription) : IRequest<string>;

    public class AskForInterestsBasedRecommendationsHandler : IRequestHandler<AskForInterestsBasedRecommendationsQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiAssistantService _aiAssistantService;

        public AskForInterestsBasedRecommendationsHandler(IUnitOfWork unitOfWork, IAiAssistantService aiAssistantService)
        {
            _unitOfWork = unitOfWork;
            _aiAssistantService = aiAssistantService;
        }

        public async Task<string> Handle(AskForInterestsBasedRecommendationsQuery request, CancellationToken ct)
        {
            var events = await _unitOfWork.EventRepository.GetAllPotentialEventsAsync(request.UserId, ct);

            if (!events.Any())
            {
                return "There are no upcoming events to suggest.";
            }

            // Build summaries
            var summaries = events
                .Select(e => $"- Name: {e.Name}, Type: {e.Type?.Name}, Description: {e.Description}, Location: {e.LocationAddress}, Starts at: {e.StartTime.AddHours(3):g}")
                .ToList();

            return await _aiAssistantService.AskForInterestsBasedSuggestionsAsync(request.UserDescription, summaries);
        }
    }
}
