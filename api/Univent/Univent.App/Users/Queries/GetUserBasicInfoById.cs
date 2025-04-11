using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Users.Dtos;

namespace Univent.App.Users.Queries
{
    public record GetUserBasicInfoByIdQuery(Guid Id) : IRequest<UserBasicInfoResponseDto>;

    public class GetUserBasicInfoByIdHandler : IRequestHandler<GetUserBasicInfoByIdQuery, UserBasicInfoResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserBasicInfoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserBasicInfoResponseDto> Handle(GetUserBasicInfoByIdQuery request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.Id, ct);

            return _mapper.Map<UserBasicInfoResponseDto>(user);
        }
    }
}
