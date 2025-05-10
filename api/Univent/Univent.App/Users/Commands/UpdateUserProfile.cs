using MediatR;
using Univent.App.Interfaces;
using Univent.App.Users.Dtos;

namespace Univent.App.Users.Commands
{
    public record UpdateUserProfileCommand(Guid Id, UpdateUserProfileRequestDto UserProfileDto) : IRequest<Unit>;

    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserProfileHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id, ct);

            user.FirstName = request.UserProfileDto.FirstName;
            user.LastName = request.UserProfileDto.LastName;
            user.Birthday = request.UserProfileDto.Birthday;
            user.UniversityId = request.UserProfileDto.UniversityId;
            user.University = null;
            user.Year = request.UserProfileDto.Year;
            user.PictureUrl = request.UserProfileDto.PictureUrl;

            await _unitOfWork.UserRepository.UpdateAsync(user, ct);

            return Unit.Value;
        }
    }
}
