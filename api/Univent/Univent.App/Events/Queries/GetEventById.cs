using AutoMapper;
using MediatR;
using Univent.App.Events.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.Events.Queries
{
    public record GetEventByIdQuery(Guid Id) : IRequest<EventFullResponseDto>;

    public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, EventFullResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventFullResponseDto> Handle(GetEventByIdQuery request, CancellationToken ct)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.Id, ct);

            var participantCount = await _unitOfWork.EventRepository.GetEventParticipantsCountAsync([eventEntity.Id], ct);

            var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync([eventEntity.Author.Id], ct);

            var eventDto = _mapper.Map<EventFullResponseDto>(eventEntity);

            // Set number of enrolled participants
            eventDto.EnrolledParticipants = participantCount.ContainsKey(eventEntity.Id)
                ? participantCount[eventEntity.Id]
                : 0;

            // Set author overall rating
            eventDto.Author.Rating = authorRatings.ContainsKey(eventEntity.Author.Id)
                ? authorRatings[eventEntity.Author.Id]
                : 0.0;

            return eventDto;
        }
    }
}
