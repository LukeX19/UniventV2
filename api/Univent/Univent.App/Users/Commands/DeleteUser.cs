using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.Users.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest<Unit>;

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            await _unitOfWork.UserRepository.DeleteAsync(request.Id, ct);

            return Unit.Value;
        }
    }
}
