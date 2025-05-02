using MediatR;
using Univent.App.Exceptions;
using Univent.App.Interfaces;
using Univent.Domain.Enums;

namespace Univent.App.Users.Commands
{
    public record ApproveUserCommand(Guid Id) : IRequest<Unit>;

    public class ApproveUserCommandHandler : IRequestHandler<ApproveUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ApproveUserCommand request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id, ct);

            if (user.Role != AppRole.Student)
            {
                throw new RestrictedRoleOperationException(request.Id);
            }

            if (user.IsAccountConfirmed == true)
            {
                throw new StatusConflictException("User", request.Id, "approved");
            }

            if (user.IsAccountBanned == true)
            {
                throw new StatusConflictException("User", request.Id, "banned");
            }

            user.IsAccountConfirmed = true;

            await _unitOfWork.UserRepository.UpdateAsync(user, ct);

            return Unit.Value;
        }
    }
}
