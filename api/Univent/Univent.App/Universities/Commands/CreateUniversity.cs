using AutoMapper;
using MediatR;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.App.Universities.Dtos;
using Univent.Domain.Models.Universities;

namespace Univent.App.Universities.Commands
{
    public record CreateUniversityCommand(UniversityRequestDto UniversityDto) : IRequest<CreateUniversityResponseDto>;

    public class CreateUniversityHandler : IRequestHandler<CreateUniversityCommand, CreateUniversityResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUniversityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUniversityResponseDto> Handle(CreateUniversityCommand request, CancellationToken ct)
        {
            var nameExists = await _unitOfWork.UniversityRepository.ExistsByNameAsync(request.UniversityDto.Name, ct);
            if (nameExists)
            {
                throw new NameConflictException(typeof(University).Name, request.UniversityDto.Name);
            }

            var university = new University()
            {
                Name = request.UniversityDto.Name
            };
            var createdUniversityId = await _unitOfWork.UniversityRepository.CreateAsync(university, ct);

            return new CreateUniversityResponseDto
            {
                Id = createdUniversityId
            };
        }
    }
}
