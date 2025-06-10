using MediatR;
using Univent.App.EventParticipants.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.EventParticipants.Queries
{
    public record GetEventParticipantFeedbackStatusQuery(Guid EventId, Guid UserId) : IRequest<EventParticipantFeedbackStatusResponseDto>;

    public class GetEventParticipantFeedbackStatusHandler : IRequestHandler<GetEventParticipantFeedbackStatusQuery, EventParticipantFeedbackStatusResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEventParticipantFeedbackStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EventParticipantFeedbackStatusResponseDto> Handle(GetEventParticipantFeedbackStatusQuery request, CancellationToken ct)
        {
            var eventParticipant = await _unitOfWork.EventParticipantRepository.GetEventParticipantByIdPairAsync(request.EventId, request.UserId, ct);

            return new EventParticipantFeedbackStatusResponseDto()
            {
                HasCompletedFeedback = eventParticipant.HasCompletedFeedback
            };
        }
    }
}
