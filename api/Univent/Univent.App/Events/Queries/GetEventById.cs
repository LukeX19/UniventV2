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

            var eventDto = _mapper.Map<EventFullResponseDto>(eventEntity);

            // Set author
            if (eventEntity.Author != null)
            {
                var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync([eventEntity.Author.Id], ct);

                eventDto.Author.Id = eventEntity.Author.Id;
                eventDto.Author.FirstName = eventEntity.Author.FirstName;
                eventDto.Author.LastName = eventEntity.Author.LastName;
                eventDto.Author.PictureUrl = eventEntity.Author.PictureUrl;
                eventDto.Author.Rating = authorRatings.ContainsKey(eventEntity.Author.Id)
                    ? authorRatings[eventEntity.Author.Id]
                    : 0.0;

            }
            else
            {
                eventDto.Author = new EventAuthorResponseDto
                {
                    Id = Guid.Empty,
                    FirstName = "Deleted",
                    LastName = "User",
                    PictureUrl = null,
                    Rating = 0.0
                };
            }

            return eventDto;
        }
    }
}
