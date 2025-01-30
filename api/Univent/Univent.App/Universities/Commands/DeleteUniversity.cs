using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.Universities.Commands
{
    public record DeleteUniversityCommand(Guid Id) : IRequest<Unit>;

    public class DeleteUniversityHandler : IRequestHandler<DeleteUniversityCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUniversityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUniversityCommand request, CancellationToken ct)
        {
            await _unitOfWork.UniversityRepository.DeleteAsync(request.Id, ct);

            return Unit.Value;
        }
    }
}
