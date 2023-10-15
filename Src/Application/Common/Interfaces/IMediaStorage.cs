namespace Application.Common.Interfaces;

public interface IMediaStorage
{
    public Task<string> UploadMediaAsync(Stream file, string fileName, CancellationToken cancellationToken = default);
}