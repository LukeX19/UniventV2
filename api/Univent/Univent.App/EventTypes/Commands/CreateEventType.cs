using MediatR;
using Univent.App.EventTypes.Dtos;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.Domain.Models.Events;

namespace Univent.App.EventTypes.Commands
{
    public record CreateEventTypeCommand(EventTypeRequestDto EventTypeDto) : IRequest<CreateEventTypeResponseDto>;

    public class CreateEventTypeHandler : IRequestHandler<CreateEventTypeCommand, CreateEventTypeResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateEventTypeResponseDto> Handle(CreateEventTypeCommand request, CancellationToken ct)
        {
            var existingEventType = await _unitOfWork.EventTypeRepository.GetByNameAsync(request.EventTypeDto.Name, ct);
            if (existingEventType != null)
            {
                throw new NameConflictWithStatusException(typeof(EventType).Name, request.EventTypeDto.Name, existingEventType.IsDeleted);
            }

            var eventType = new EventType()
            {
                Name = request.EventTypeDto.Name,
                IsDeleted = false
            };
            var createdEventTypeId = await _unitOfWork.EventTypeRepository.CreateAsync(eventType, ct);

            return new CreateEventTypeResponseDto
            {
                Id = createdEventTypeId
            };
        }
    }
}
