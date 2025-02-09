using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Universities.Dtos;

namespace Univent.App.Universities.Queries
{
    public record SearchUniversityQuery(string Query) : IRequest<ICollection<UniversityResponseDto>>;

    public class SearchUniversityHandler : IRequestHandler<SearchUniversityQuery, ICollection<UniversityResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchUniversityHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<UniversityResponseDto>> Handle(SearchUniversityQuery request, CancellationToken ct)
        {
            var universities = await _unitOfWork.UniversityRepository.SearchUniversityAsync(request.Query, ct);

            return _mapper.Map<ICollection<UniversityResponseDto>>(universities);
        }
    }
}
