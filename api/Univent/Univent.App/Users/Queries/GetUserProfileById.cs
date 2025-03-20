using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            var user = await _unitOfWork.UserRepository.GetUserById(request.Id, ct);

            var authorRatings = await _unitOfWork.UserRepository.GetAverageRatingsAsync([request.Id], ct);

            var userDto = _mapper.Map<UserProfileResponseDto>(user);

            // Set user overall rating
            userDto.Rating = authorRatings.ContainsKey(request.Id)
                ? authorRatings[request.Id]
                : 0.0;

            return userDto;
        }
    }
}
