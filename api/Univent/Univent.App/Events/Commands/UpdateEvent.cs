using MediatR;
using Univent.App.Events.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.Events.Commands
{
    public record UpdateEventCommand(Guid Id, UpdateEventRequestDto EventDto) : IRequest<Unit>;

    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken ct)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.Id, ct);

            eventEntity.Name = request.EventDto.Name;
            eventEntity.Description = request.EventDto.Description;
            eventEntity.MaximumParticipants = request.EventDto.MaximumParticipants;
            eventEntity.StartTime = request.EventDto.StartTime;
            eventEntity.LocationAddress = request.EventDto.LocationAddress;
            eventEntity.LocationLat = request.EventDto.LocationLat;
            eventEntity.LocationLong = request.EventDto.LocationLong;
            eventEntity.PictureUrl = request.EventDto.PictureUrl;

            await _unitOfWork.EventRepository.UpdateAsync(eventEntity, ct);

            return Unit.Value;
        }
    }
}
