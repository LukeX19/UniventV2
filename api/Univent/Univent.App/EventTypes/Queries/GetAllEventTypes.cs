using AutoMapper;
using MediatR;
using Univent.App.EventTypes.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.EventTypes.Queries
{
    public record GetAllEventTypesQuery() : IRequest<ICollection<EventTypeResponseDto>>;

    public class GetAllEventTypesHandler : IRequestHandler<GetAllEventTypesQuery, ICollection<EventTypeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventTypesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<EventTypeResponseDto>> Handle(GetAllEventTypesQuery request, CancellationToken ct)
        {
            var eventTypes = await _unitOfWork.EventTypeRepository.GetAllAsync(ct);

            return _mapper.Map<ICollection<EventTypeResponseDto>>(eventTypes);
        }
    }
}
