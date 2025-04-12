using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.Api.Extensions;
using Univent.App.Events.Commands;
using Univent.App.Events.Dtos;
using Univent.App.Events.Queries;
using Univent.App.Pagination.Dtos;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/events")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventRequestDto eventDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var command = new CreateEventCommand(userId, eventDto);
            var response = await _mediator.Send(command);

            return Created($"/api/events/{response}", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventsSummaries([FromQuery] PaginationRequestDto pagination,
            [FromQuery] string? search, [FromQuery] ICollection<Guid>? types)
        {
            var query = new GetAllEventsSummariesQuery(pagination, search, types);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("created-by/{userId}")]
        public async Task<IActionResult> GetCreatedEventsSummariesByUserId(Guid userId, [FromQuery] PaginationRequestDto pagination)
        {
            var query = new GetCreatedEventsSummariesByUserIdQuery(userId, pagination);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("participated-by/{userId}")]
        public async Task<IActionResult> GetParticipatedEventsSummariesByUserId(Guid userId, [FromQuery] PaginationRequestDto pagination)
        {
            var query = new GetParticipatedEventsSummariesByUserIdQuery(userId, pagination);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var query = new GetEventByIdQuery(id);
            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
