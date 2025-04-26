using MediatR;
using Univent.App.Exceptions;
using Univent.App.Interfaces;

namespace Univent.App.EventParticipants.Commands
{
    public record DeleteEventParticipantCommand(Guid EventId, Guid UserId) : IRequest<Unit>;

    public class DeleteEventParticipantHandler : IRequestHandler<DeleteEventParticipantCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventParticipantHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteEventParticipantCommand request, CancellationToken ct)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.EventId, ct);

            if (DateTime.UtcNow > eventEntity.StartTime.AddHours(-2))
            {
                throw new EventWithdrawalClosedException(eventEntity.Id);
            }

            await _unitOfWork.EventParticipantRepository.DeleteEventParticipantAsync(request.EventId, request.UserId, ct);

            return Unit.Value;
        }
    }
}
