using MediatR;
using Univent.App.Files.Dtos;
using Univent.App.Interfaces;

namespace Univent.App.Files.Commands
{
    public record UploadFileCommand(UploadFileRequestDto FileDto) : IRequest<FileResponseDto>;

    public class UploadFileHandler : IRequestHandler<UploadFileCommand, FileResponseDto>
    {
        private readonly IFileService _fileService;

        public UploadFileHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<FileResponseDto> Handle(UploadFileCommand request, CancellationToken ct)
        {
            using var stream = request.FileDto.File.OpenReadStream();
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.FileDto.File.FileName)}";
            var fileUrl = await _fileService.UploadAsync(stream, fileName, ct);

            return new FileResponseDto
            {
                Url = fileUrl
            };
        }
    }
}
