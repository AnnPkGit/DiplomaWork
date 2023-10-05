namespace Application.Common.Interfaces;

public interface IMediaService
{
    public Task<string> UploadMediaAsync(Stream file, string fileName, CancellationToken cancellationToken = default);
}