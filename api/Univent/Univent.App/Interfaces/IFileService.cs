namespace Univent.App.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadAsync(Stream stream, string fileName, CancellationToken ct = default);
        Task DeleteAsync(string fileName, CancellationToken ct = default);
    }
}
