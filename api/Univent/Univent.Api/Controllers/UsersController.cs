using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.Api.Extensions;
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
    }
}
