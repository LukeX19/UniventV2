using MediatR;
using Univent.App.EventTypes.Dtos;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.Domain.Models.Events;

namespace Univent.App.EventTypes.Commands
{
    public record UpdateEventTypeCommand(Guid Id, EventTypeRequestDto EventTypeDto) : IRequest<Unit>;

    public class UpdateEventTypeHandler : IRequestHandler<UpdateEventTypeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEventTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateEventTypeCommand request, CancellationToken ct)
        {
            var eventType = await _unitOfWork.EventTypeRepository.GetByIdAsync(request.Id, ct);

            var existingEventType = await _unitOfWork.EventTypeRepository.GetByNameAsync(request.EventTypeDto.Name, ct);
            if (existingEventType != null && existingEventType.Id != request.Id)
            {
                throw new NameConflictWithStatusException(typeof(EventType).Name, request.EventTypeDto.Name, existingEventType.IsDeleted);
            }

            eventType.Name = request.EventTypeDto.Name;

            await _unitOfWork.EventTypeRepository.UpdateAsync(eventType, ct);

            return Unit.Value;
        }
    }
}
