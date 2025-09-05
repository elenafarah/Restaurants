namespace Restaurants.Infrastructure.Seeders;

public interface IIdentitySeeder
{
  static abstract Task SeedAsync(IServiceProvider sp, CancellationToken ct = default);
}

