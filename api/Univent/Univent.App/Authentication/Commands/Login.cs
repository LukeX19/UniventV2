using MediatR;
using Univent.App.Authentication.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.Authentication.Commands
{
    public record LoginCommand(LoginRequestDto LoginDto) : IRequest<AuthenticationResponseDto>;

    public class LoginHandler : IRequestHandler<LoginCommand, AuthenticationResponseDto>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IIdentityService _identityService;

        public LoginHandler(IAuthenticationService authenticationService, IIdentityService identityService)
        {
            _authenticationService = authenticationService;
            _identityService = identityService;
        }

        public async Task<AuthenticationResponseDto> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _authenticationService.LoginAsync(request.LoginDto.Email, request.LoginDto.Password, ct);

            var claimsIdentity = _identityService.CreateClaimsIdentity(user);
            var token = _identityService.CreateSecurityToken(claimsIdentity);

            return new AuthenticationResponseDto
            {
                UserId = user.Id,
                Token = token
            };
        }
    }
}
