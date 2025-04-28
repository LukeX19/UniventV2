using AutoMapper;
using MediatR;
using Univent.App.EventParticipants.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.EventParticipants.Queries
{
    public record GetEventParticipantsByEventIdQuery(Guid EventId) : IRequest<ICollection<EventParticipantFullResponseDto>>;

    public class GetEventParticipantsByEventIdHandler : IRequestHandler<GetEventParticipantsByEventIdQuery, ICollection<EventParticipantFullResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipantsByEventIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<EventParticipantFullResponseDto>> Handle(GetEventParticipantsByEventIdQuery request, CancellationToken ct)
        {
            var eventParticipants = await _unitOfWork.EventParticipantRepository.GetEventParticipantsByEventIdAsync(request.EventId, ct);

            var userIds = eventParticipants.Select(ep => ep.UserId).Distinct().ToList();

            var userRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync(userIds, ct);

            var result = eventParticipants.Select(ep =>
            {
                var dto = _mapper.Map<EventParticipantFullResponseDto>(ep);

                dto.Rating = userRatings.ContainsKey(ep.UserId)
                    ? userRatings[ep.UserId]
                    : 0.0;

                return dto;
            }).ToList();

            return result;
        }
    }
}
