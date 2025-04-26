using MediatR;
using Univent.App.EventParticipants.Dtos;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.Domain.Models.Associations;

namespace Univent.App.EventParticipants.Commands
{
    public record CreateEventParticipantCommand(Guid EventId, Guid UserId) : IRequest<CreateEventParticipantResponseDto>;

    public class CreateEventParticipantHandler : IRequestHandler<CreateEventParticipantCommand, CreateEventParticipantResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventParticipantHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateEventParticipantResponseDto> Handle(CreateEventParticipantCommand request, CancellationToken ct)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.EventId, ct);

            if (eventEntity.AuthorId == request.UserId)
            {
                throw new EventAuthorEnrollmentException();
            }

            var currentParticipantsCount = await _unitOfWork.EventParticipantRepository.CountEventParticipantsByEventIdAsync(request.EventId, ct);

            if (currentParticipantsCount >= eventEntity.MaximumParticipants)
            {
                throw new EventMaximumParticipantsReachedException();
            }

            if (DateTime.UtcNow > eventEntity.StartTime.AddHours(-2))
            {
                throw new EventEnrollmentClosedException(eventEntity.Id);
            }

            var eventParticipant = new EventParticipant()
            {
                EventId = request.EventId,
                UserId = request.UserId,
                HasCompletedFeedback = false
            };
            var (eventId, userId) = await _unitOfWork.EventParticipantRepository.CreateEventParticipantAsync(eventParticipant, ct);

            return new CreateEventParticipantResponseDto
            {
                EventId = eventId,
                UserId = userId
            };
        }
    }
}
