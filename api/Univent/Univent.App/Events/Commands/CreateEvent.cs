using MediatR;
using Univent.App.Events.Dtos;
using Univent.App.Interfaces;
using Univent.Domain.Models.Events;

namespace Univent.App.Events.Commands
{
    public record CreateEventCommand(Guid AuthorId, CreateEventRequestDto EventDto) : IRequest<CreateEventResponseDto>;

    public class CreateEventHandler : IRequestHandler<CreateEventCommand, CreateEventResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateEventResponseDto> Handle(CreateEventCommand request, CancellationToken ct)
        {
            var newEvent = new Event()
            {
                Name = request.EventDto.Name,
                Description = request.EventDto.Description,
                MaximumParticipants = request.EventDto.MaximumParticipants,
                StartTime = request.EventDto.StartTime,
                LocationAddress = request.EventDto.LocationAddress,
                LocationLat = request.EventDto.LocationLat,
                LocationLong = request.EventDto.LocationLong,
                PictureUrl = request.EventDto.PictureUrl,
                IsCancelled = false,
                TypeId = request.EventDto.TypeId,
                AuthorId = request.AuthorId
            };
            var createdEventId = await _unitOfWork.EventRepository.CreateAsync(newEvent, ct);

            return new CreateEventResponseDto
            {
                Id = createdEventId
            };
        }
    }
}
