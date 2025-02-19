using AutoMapper;
using MediatR;
using Univent.App.EventTypes.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.EventTypes.Queries
{
    public record GetAllActiveEventTypesQuery() : IRequest<ICollection<EventTypeResponseDto>>;

    public class GetAllActiveEventTypesHandler : IRequestHandler<GetAllActiveEventTypesQuery, ICollection<EventTypeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllActiveEventTypesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<EventTypeResponseDto>> Handle(GetAllActiveEventTypesQuery request, CancellationToken ct)
        {
            var eventTypes = await _unitOfWork.EventTypeRepository.GetAllActiveAsync(ct);

            return _mapper.Map<ICollection<EventTypeResponseDto>>(eventTypes);
        }
    }
}
