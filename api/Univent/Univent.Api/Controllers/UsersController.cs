using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.Api.Extensions;
using Univent.App.Pagination.Dtos;
using Univent.App.Users.Commands;
using Univent.App.Users.Dtos;
using Univent.App.Users.Queries;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var query = new GetUserBasicInfoByIdQuery(userId);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("profile/{id}")]
        public async Task<IActionResult> GetUserProfileById(Guid id)
        {
            var query = new GetUserProfileByIdQuery(id);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationRequestDto pagination)
        {
            var query = new GetAllUsersQuery(pagination);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}/approve")]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> ApproveUser(Guid id)
        {
            var command = new ApproveUserCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}/ban")]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> BanUser(Guid id)
        {
            var command = new BanUserCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPut]
        [Route("profile")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileRequestDto userProfileDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new UpdateUserProfileCommand(userId, userProfileDto);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
