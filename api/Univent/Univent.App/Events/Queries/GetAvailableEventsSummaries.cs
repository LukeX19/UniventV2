using AutoMapper;
using MediatR;
using Univent.App.Events.Dtos;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;

namespace Univent.App.Events.Queries
{
    public record GetAvailableEventsSummariesQuery(PaginationRequestDto Pagination,
        string? Search, ICollection<Guid>? Types) : IRequest<PaginationResponseDto<EventSummaryResponseDto>>;

    public class GetAvailableEventsSummariesHandler : IRequestHandler<GetAvailableEventsSummariesQuery, PaginationResponseDto<EventSummaryResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableEventsSummariesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<EventSummaryResponseDto>> Handle(GetAvailableEventsSummariesQuery request, CancellationToken ct)
        {
            var paginatedEvents = await _unitOfWork.EventRepository.GetAvailableEventsSummariesAsync(request.Pagination, request.Search, request.Types, ct);

            var eventIds = paginatedEvents.Elements.Select(e => e.Id).ToList();
            var participantCounts = await _unitOfWork.EventRepository.GetEventParticipantsCountAsync(eventIds, ct);

            var authorIds = paginatedEvents.Elements
                .Where(e => e.Author != null)
                .Select(e => e.Author.Id)
                .Distinct()
                .ToList();
            var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync(authorIds, ct);

            var eventDtos = paginatedEvents.Elements.Select(eventEntity =>
            {
                var dto = _mapper.Map<EventSummaryResponseDto>(eventEntity);

                // Set number of enrolled participants
                dto.EnrolledParticipants = participantCounts.ContainsKey(eventEntity.Id)
                    ? participantCounts[eventEntity.Id]
                    : 0;

                // Set author
                if (eventEntity.Author != null)
                {
                    dto.Author.Id = eventEntity.Author.Id;
                    dto.Author.FirstName = eventEntity.Author.FirstName;
                    dto.Author.LastName = eventEntity.Author.LastName;
                    dto.Author.PictureUrl = eventEntity.Author.PictureUrl;
                    dto.Author.Rating = authorRatings.ContainsKey(eventEntity.Author.Id)
                        ? authorRatings[eventEntity.Author.Id]
                        : 0.0;

                }
                else
                {
                    dto.Author = new EventAuthorResponseDto
                    {
                        Id = Guid.Empty,
                        FirstName = "Unknown",
                        LastName = "User",
                        PictureUrl = null,
                        Rating = 0.0
                    };
                }

                return dto;
            }).ToList();

            return new PaginationResponseDto<EventSummaryResponseDto>(
                eventDtos,
                paginatedEvents.PageIndex,
                paginatedEvents.TotalPages,
                paginatedEvents.ResultsCount);
        }
    }
}
