using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.App.AiAssistant.Queries;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/ai-assistant")]
    [Authorize]
    public class AiAssistantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AiAssistantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("events/interests/recommend")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AskForInterestsBasedRecommendations([FromBody] string userDescription)
        {
            var query = new AskForInterestsBasedRecommendationsQuery(userDescription);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        [Route("events/location/recommend")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AskForLocationBasedRecommendations([FromBody] string locationDescription)
        {
            var query = new AskForLocationBasedRecommendationsQuery(locationDescription);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        [Route("events/time/recommend")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AskForTimeBasedRecommendations([FromBody] string timePreference)
        {
            var query = new AskQuestionAboutTimePreferencesQuery(timePreference);
            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
