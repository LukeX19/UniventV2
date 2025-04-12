﻿using AutoMapper;
using MediatR;
using Univent.App.Events.Dtos;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;

namespace Univent.App.Events.Queries
{
    public record GetAllEventsSummariesQuery(PaginationRequestDto Pagination,
        string? Search, ICollection<Guid>? Types) : IRequest<PaginationResponseDto<EventSummaryResponseDto>>;

    public class GetAllEventsSummariesHandler : IRequestHandler<GetAllEventsSummariesQuery, PaginationResponseDto<EventSummaryResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventsSummariesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<EventSummaryResponseDto>> Handle(GetAllEventsSummariesQuery request, CancellationToken ct)
        {
            var paginatedEvents = await _unitOfWork.EventRepository.GetAllEventsSummariesAsync(request.Pagination, request.Search, request.Types, ct);

            var eventIds = paginatedEvents.Elements.Select(e => e.Id).ToList();
            var participantCounts = await _unitOfWork.EventRepository.GetEventParticipantsCountAsync(eventIds, ct);

            var authorIds = paginatedEvents.Elements.Select(e => e.Author.Id).Distinct().ToList();
            var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync(authorIds, ct);

            var eventDtos = paginatedEvents.Elements.Select(eventEntity =>
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

            return new PaginationResponseDto<EventSummaryResponseDto>(
                eventDtos,
                paginatedEvents.PageIndex,
                paginatedEvents.TotalPages,
                paginatedEvents.ResultsCount);
        }
    }
}
