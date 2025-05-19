using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.App.AiAssistant.Queries;
using Univent.App.Interfaces;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/ai-assistant")]
    [Authorize]
    public class AiAssistantController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWeatherService _weatherService;

        public AiAssistantController(IMediator mediator, IWeatherService weatherService)
        {
            _mediator = mediator;
            _weatherService = weatherService;
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

        [HttpPost]
        [Route("events/weather/recommend")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AskForWeatherBasedRecommendations()
        {
            var query = new AskForWeatherBasedRecommendationsQuery();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("timisoara")]
        public async Task<IActionResult> GetWeatherForTimisoara()
        {
            var result = await _weatherService.Get8DaysForecastAsync(45.7489, 21.2087);
            return Ok(result);
        }
    }
}
