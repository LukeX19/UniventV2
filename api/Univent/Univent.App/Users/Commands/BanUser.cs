using MediatR;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.Domain.Enums;

namespace Univent.App.Users.Commands
{
    public record BanUserCommand(Guid Id) : IRequest<Unit>;

    public class BanUserCommandHandler : IRequestHandler<BanUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BanUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(BanUserCommand request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id, ct);

            if (user.Role != AppRole.Student)
            {
                throw new RestrictedRoleOperationException(request.Id);
            }

            if (user.IsAccountBanned == true)
            {
                throw new StatusConflictException("User", request.Id, "banned");
            }
            user.IsAccountBanned = true;

            await _unitOfWork.UserRepository.UpdateAsync(user, ct);

            return Unit.Value;
        }
    }
}
