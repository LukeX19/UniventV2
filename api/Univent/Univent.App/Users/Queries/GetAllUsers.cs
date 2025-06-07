using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;
using Univent.App.Users.Dtos;

namespace Univent.App.Users.Queries
{
    public record GetAllUsersQuery(PaginationRequestDto Pagination) : IRequest<PaginationResponseDto<UserManagementResponseDto>>;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, PaginationResponseDto<UserManagementResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<UserManagementResponseDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
        {
            var paginatedUsers = await _unitOfWork.UserRepository.GetAllUsersAsync(request.Pagination, ct);

            var userDtos = paginatedUsers.Elements.Select(userEntity =>
            {
                var dto = _mapper.Map<UserManagementResponseDto>(userEntity);

                // Handle null university
                dto.UniversityName = userEntity.University != null
                    ? userEntity.University.Name
                    : "Unknown University";

                return dto;
            }).ToList();

            return new PaginationResponseDto<UserManagementResponseDto>(
                userDtos,
                paginatedUsers.PageIndex,
                paginatedUsers.TotalPages,
                paginatedUsers.ResultsCount);
        }
    }
}
