using Microsoft.AspNetCore.Http;

namespace Univent.App.Files.Dtos
{
    public class UploadFileRequestDto
    {
        public IFormFile File { get; set; }
    }
}
