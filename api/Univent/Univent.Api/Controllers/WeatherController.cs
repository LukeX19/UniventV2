using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.App.Weather.Queries;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/weather")]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get8DaysForecast()
        {
            var query = new Get8DaysForecastQuery();
            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
