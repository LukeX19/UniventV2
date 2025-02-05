using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univent.App.Files.Commands;
using Univent.App.Files.Dtos;

namespace Univent.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequestDto fileDto)
        {
            var command = new UploadFileCommand(fileDto);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var command = new DeleteFileCommand(fileName);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
