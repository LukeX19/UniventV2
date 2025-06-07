using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Users.Dtos;

namespace Univent.App.Users.Queries
{
    public record GetUserProfileByIdQuery(Guid Id) : IRequest<UserProfileResponseDto>;

    public class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserProfileByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileResponseDto> Handle(GetUserProfileByIdQuery request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id, ct);

            var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync([request.Id], ct);

            var createdEventsCounter = await _unitOfWork.EventRepository.GetCreatedEventsCountByUserIdAsync(request.Id);
            var participatedEventsCounter = await _unitOfWork.EventRepository.GetParticipatedEventsCountByUserIdAsync(request.Id);

            var userDto = _mapper.Map<UserProfileResponseDto>(user);

            // Set user overall rating
            userDto.Rating = authorRatings.ContainsKey(request.Id)
                ? authorRatings[request.Id]
                : 0.0;

            // Set created events counter
            userDto.CreatedEvents = createdEventsCounter;

            // Set events participation counter
            userDto.Participations = participatedEventsCounter;

            // Handle null university
            userDto.UniversityName = user.University != null
                ? user.University.Name
                : "Unknown University";

            // Defensive fallback in case of null
            userDto.UniversityId = user.University?.Id ?? Guid.Empty;

            return userDto;
        }
    }
}
