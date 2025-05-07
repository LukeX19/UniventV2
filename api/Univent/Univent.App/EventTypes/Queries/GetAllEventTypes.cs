using AutoMapper;
using MediatR;
using Univent.App.EventTypes.Dtos;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;

namespace Univent.App.EventTypes.Queries
{
    public record GetAllEventTypesQuery(PaginationRequestDto Pagination) : IRequest<PaginationResponseDto<EventTypeResponseDto>>;

    public class GetAllEventTypesHandler : IRequestHandler<GetAllEventTypesQuery, PaginationResponseDto<EventTypeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventTypesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<EventTypeResponseDto>> Handle(GetAllEventTypesQuery request, CancellationToken ct)
        {
            var paginatedEventTypes = await _unitOfWork.EventTypeRepository.GetAllAsync(request.Pagination, ct);

            var eventTypeDtos = paginatedEventTypes.Elements.Select(eventTypeEntity =>
            {
                var dto = _mapper.Map<EventTypeResponseDto>(eventTypeEntity);

                return dto;
            }).ToList();

            return new PaginationResponseDto<EventTypeResponseDto>(
                eventTypeDtos,
                paginatedEventTypes.PageIndex,
                paginatedEventTypes.TotalPages,
                paginatedEventTypes.ResultsCount);
        }
    }
}
