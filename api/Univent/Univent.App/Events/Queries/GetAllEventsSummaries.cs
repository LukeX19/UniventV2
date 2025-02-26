using AutoMapper;
using MediatR;
using Univent.App.Events.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.Events.Queries
{
    public record GetAllEventsSummariesQuery() : IRequest<ICollection<EventSummaryResponseDto>>;

    public class GetAllEventsSummariesHandler : IRequestHandler<GetAllEventsSummariesQuery, ICollection<EventSummaryResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventsSummariesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<EventSummaryResponseDto>> Handle(GetAllEventsSummariesQuery request, CancellationToken ct)
        {
            var events = await _unitOfWork.EventRepository.GetAllEventsSummariesAsync(ct);

            var eventIds = events.Select(e => e.Id).ToList();

            var participantCounts = await _unitOfWork.EventRepository.GetEventParticipantsCountAsync(eventIds, ct);

            var authorIds = events.Select(e => e.Author.Id).Distinct().ToList();

            var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync(authorIds, ct);

            var eventsDtos = events.Select(eventEntity =>
            {
                var dto = _mapper.Map<EventSummaryResponseDto>(eventEntity);

                // Set number of enrolled participants
                dto.EnrolledParticipants = participantCounts.ContainsKey(eventEntity.Id)
                    ? participantCounts[eventEntity.Id]
                    : 0;

                // Set author overall rating
                dto.Author.Rating = authorRatings.ContainsKey(eventEntity.Author.Id)
                    ? authorRatings[eventEntity.Author.Id]
                    : 0.0;

                return dto;
            }).ToList();

            return eventsDtos;
        }
    }
}
