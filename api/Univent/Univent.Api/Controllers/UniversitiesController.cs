using MediatR;
using Microsoft.AspNetCore.Mvc;
using Univent.App.Universities.Commands;
using Univent.App.Universities.Dtos;
using Univent.App.Universities.Queries;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/universities")]
    public class UniversitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UniversitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversity(UniversityRequestDto universityDto)
        {
            var command = new CreateUniversityCommand(universityDto);
            var response = await _mediator.Send(command);

            return Created($"/api/universities/{response}", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUniversities()
        {
            var query = new GetAllUniversitiesQuery();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUniversity(Guid id, UniversityRequestDto universityDto)
        {
            var command = new UpdateUniversityCommand(id, universityDto);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUniversity(Guid id)
        {
            var command = new DeleteUniversityCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
