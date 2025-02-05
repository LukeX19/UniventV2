using MediatR;
using Univent.App.Interfaces;

namespace Univent.App.Files.Commands
{
    public record DeleteFileCommand(string FileName) : IRequest<Unit>;

    public class DeleteFileHandler : IRequestHandler<DeleteFileCommand, Unit>
    {
        private readonly IFileService _fileService;

        public DeleteFileHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken ct)
        {
            await _fileService.DeleteAsync(request.FileName, ct);

            return Unit.Value;
        }
    }
}
