using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univent.App.AiAssistant.Queries;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/ai-assistant")]
    public class AiAssistantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AiAssistantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("events/ask")]
        public async Task<IActionResult> AskQuestionAboutHotelReviews([FromBody] string question)
        {
            var query = new AskQuestionAboutEventsQuery(question);
            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
