namespace Restaurants.Domain.Repositories;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(string fileName, Stream file);

    public string? GetBlobSasUrl(string? blobUrl);
}

