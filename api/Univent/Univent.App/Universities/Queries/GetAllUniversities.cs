using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;
using Univent.App.Universities.Dtos;

namespace Univent.App.Universities.Queries
{
    public record GetAllUniversitiesQuery(PaginationRequestDto Pagination) : IRequest<PaginationResponseDto<UniversityResponseDto>>;

    public class GetAllUniversitiesHandler : IRequestHandler<GetAllUniversitiesQuery, PaginationResponseDto<UniversityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUniversitiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<UniversityResponseDto>> Handle(GetAllUniversitiesQuery request, CancellationToken ct)
        {
            var paginatedUniversities = await _unitOfWork.UniversityRepository.GetAllAsync(request.Pagination, ct);

            var universityDtos = paginatedUniversities.Elements.Select(universityEntity =>
            {
                var dto = _mapper.Map<UniversityResponseDto>(universityEntity);

                return dto;
            }).ToList();

            return new PaginationResponseDto<UniversityResponseDto>(
                universityDtos,
                paginatedUniversities.PageIndex,
                paginatedUniversities.TotalPages,
                paginatedUniversities.ResultsCount);
        }
    }
}
