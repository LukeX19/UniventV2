using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.App.EventTypes.Commands;
using Univent.App.EventTypes.Dtos;
using Univent.App.EventTypes.Queries;
using Univent.App.Pagination.Dtos;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/event-types")]
    public class EventTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> CreateEventType(EventTypeRequestDto eventTypeDto)
        {
            var command = new CreateEventTypeCommand(eventTypeDto);
            var response = await _mediator.Send(command);

            return Created($"/api/event-types/{response}", response);
        }

        [HttpGet]
        [Route("active")]
        [Authorize]
        public async Task<IActionResult> GetAllActiveEventTypes()
        {
            var query = new GetAllActiveEventTypesQuery();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> GetAllEventTypes([FromQuery] PaginationRequestDto pagination)
        {
            var query = new GetAllEventTypesQuery(pagination);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> UpdateEventType(Guid id, EventTypeRequestDto eventTypeDto)
        {
            var command = new UpdateEventTypeCommand(id, eventTypeDto);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}/enable")]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> EnableEventType(Guid id)
        {
            var command = new EnableEventTypeCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}/disable")]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> DisableEventType(Guid id)
        {
            var command = new DisableEventTypeCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
