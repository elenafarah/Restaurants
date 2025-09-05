namespace Restaurants.Infrastructure.Configuration;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;

    public string AccountKey { get; set; } = default!;

    public string AccountName { get; set; } = default!;
}

