using MediatR;
using Univent.App.Authentication.Dtos;
using Univent.App.Interfaces;
using Univent.Domain.Models.Users;

namespace Univent.App.Authentication.Commands
{
    public record RegisterCommand(RegisterRequestDto RegisterDto) : IRequest<AuthenticationResponseDto>;

    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthenticationResponseDto>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IIdentityService _identityService;

        public RegisterHandler(IAuthenticationService authenticationService, IIdentityService identityService)
        {
            _authenticationService = authenticationService;
            _identityService = identityService;
        }

        public async Task<AuthenticationResponseDto> Handle(RegisterCommand request, CancellationToken ct)
        {
            var user = new AppUser()
            {
                Email = request.RegisterDto.Email,
                UserName = request.RegisterDto.Email,
                FirstName = request.RegisterDto.Email,
                LastName = request.RegisterDto.Email,
                Birthday = request.RegisterDto.Birthday,
                PictureUrl = request.RegisterDto.PictureURL,
                Role = request.RegisterDto.Role,
                Year = request.RegisterDto.Year,
                UniversityId = request.RegisterDto.UniversityId,
                IsAccountConfirmed = false
            };

            var createdUser = await _authenticationService.RegisterAsync(user, request.RegisterDto.Password, ct);

            var claimsIdentity = _identityService.CreateClaimsIdentity(createdUser);
            var token = _identityService.CreateSecurityToken(claimsIdentity);

            return new AuthenticationResponseDto
            {
                UserId = createdUser.Id,
                Token = token
            };
        }
    }
}
