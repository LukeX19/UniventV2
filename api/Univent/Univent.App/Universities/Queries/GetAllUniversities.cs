using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Universities.Dtos;

namespace Univent.App.Universities.Queries
{
    public record GetAllUniversitiesQuery() : IRequest<ICollection<UniversityResponseDto>>;

    public class GetAllUniversitiesHandler : IRequestHandler<GetAllUniversitiesQuery, ICollection<UniversityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUniversitiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<UniversityResponseDto>> Handle(GetAllUniversitiesQuery request, CancellationToken ct)
        {
            var universities = await _unitOfWork.UniversityRepository.GetAllAsync(ct);

            return _mapper.Map<ICollection<UniversityResponseDto>>(universities);
        }
    }
}
