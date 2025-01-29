using MediatR;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.App.Universities.Dtos;
using Univent.Domain.Models.Universities;

namespace Univent.App.Universities.Commands
{
    public record UpdateUniversityCommand(Guid Id, UniversityRequestDto UniversityDto) : IRequest<Unit>;

    public class UpdateUniversityHandler : IRequestHandler<UpdateUniversityCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUniversityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUniversityCommand request, CancellationToken ct)
        {
            var university = await _unitOfWork.UniversityRepository.GetByIdAsync(request.Id, ct);

            var nameExists = await _unitOfWork.UniversityRepository.ExistsByNameAsync(request.UniversityDto.Name, ct);
            if (nameExists && university.Name != request.UniversityDto.Name)
            {
                throw new NameConflictException(typeof(University).Name, request.UniversityDto.Name);
            }

            university.Name = request.UniversityDto.Name;

            await _unitOfWork.UniversityRepository.UpdateAsync(university, ct);

            return Unit.Value;
        }
    }
}
