using AutoMapper;
using MediatR;
using Univent.App.Interfaces;
using Univent.App.Users.Dtos;

namespace Univent.App.Users.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserResponseDto>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.Id, ct);

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
