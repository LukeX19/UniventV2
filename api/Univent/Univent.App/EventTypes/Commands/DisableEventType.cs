using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.EventTypes.Commands
{
    public record DisableEventTypeCommand(Guid Id) : IRequest<Unit>;

    public class ToggleEventTypeStatusHandler : IRequestHandler<DisableEventTypeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleEventTypeStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DisableEventTypeCommand request, CancellationToken ct)
        {
            var eventType = await _unitOfWork.EventTypeRepository.GetByIdAsync(request.Id, ct);

            eventType.IsDeleted = true;

            await _unitOfWork.EventTypeRepository.UpdateAsync(eventType, ct);

            return Unit.Value;
        }
    }
}
