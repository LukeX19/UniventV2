using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.Api.Extensions;
using Univent.App.Feedbacks.Commands;
using Univent.App.Feedbacks.Dtos;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/feedbacks")]
    [Authorize]
    public class FeedbacksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FeedbacksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("event/{eventId}/submit")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateMultipleFeedbacks(Guid eventId, [FromBody] CreateMultipleFeedbacksDto feedbacksDto)
        {
            var senderUserId = HttpContext.GetUserIdClaimValue();

            var command = new CreateMultipleFeedbacksCommand(senderUserId, eventId, feedbacksDto);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
