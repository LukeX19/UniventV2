using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.Api.Extensions;
using Univent.App.EventParticipants.Commands;
using Univent.App.EventParticipants.Queries;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/event-participants")]
    [Authorize]
    public class EventParticipantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventParticipantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{eventId}")]
        public async Task<IActionResult> CreateEventParticipant(Guid eventId)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new CreateEventParticipantCommand(eventId, userId);
            var response = await _mediator.Send(command);

            return Created($"/api/event-participants/{response.EventId}/{response.UserId}", response);
        }

        [HttpGet]
        [Route("{eventId}/participants")]
        public async Task<IActionResult> GetEventParticipantsByEventId(Guid eventId)
        {
            var query = new GetEventParticipantsByEventIdQuery(eventId);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{eventId}")]
        public async Task<IActionResult> DeleteEventParticipant(Guid eventId)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new DeleteEventParticipantCommand(eventId, userId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
