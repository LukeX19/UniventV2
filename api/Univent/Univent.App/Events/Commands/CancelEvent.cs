using MediatR;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.Domain.Models.Events;

namespace Univent.App.Events.Commands
{
    public record CancelEventCommand(Guid Id) : IRequest<Unit>;

    public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelEventCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelEventCommand request, CancellationToken ct)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.Id, ct);

            if (eventEntity.IsCancelled == true)
            {
                throw new StatusConflictException(nameof(Event), request.Id, "cancelled");
            }
            eventEntity.IsCancelled = true;

            await _unitOfWork.EventRepository.UpdateAsync(eventEntity, ct);

            return Unit.Value;
        }
    }
}
