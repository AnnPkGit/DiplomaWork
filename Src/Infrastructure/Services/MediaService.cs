using Application.Common.Interfaces;
using Azure.Storage;
using Azure.Storage.Blobs;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class MediaService : IMediaService
{
    private readonly AzureStorageConfig _storageConfig;

    public MediaService(IOptions<AzureStorageConfig> options)
    {
        _storageConfig = options.Value;
    }

    public async Task<string> UploadMediaAsync(Stream file, string fileName, CancellationToken token)
    {
        var blobUri = new Uri("https://" +
                              _storageConfig.AccountName +
                              ".blob.core.windows.net/" +
                              _storageConfig.ImageContainer +
                              "/" + fileName);
        
        var storageCredentials =
            new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
        
        var blobClient = new BlobClient(blobUri, storageCredentials);
        
        await blobClient.UploadAsync(file, token);
        return blobUri.ToString();
    }
}