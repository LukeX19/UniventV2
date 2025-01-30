using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univent.App.Authentication.Commands;
using Univent.App.Authentication.Dtos;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerDto)
        {
            var command = new RegisterCommand(registerDto);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            var command = new LoginCommand(loginDto);
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
