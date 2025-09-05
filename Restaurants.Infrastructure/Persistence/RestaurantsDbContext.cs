using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

public class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : IdentityDbContext<User>(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>().OwnsOne(r => r.Address);

        modelBuilder.Entity<Restaurant>()
            .Navigation(r => r.Address)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .HasMany(d => d.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId);

        modelBuilder.Entity<Restaurant>()
            .HasIndex(r => r.Name);

        modelBuilder.Entity<Restaurant>()
            .HasIndex(r => r.Description);

        modelBuilder.Entity<Dish>()
            .Property(d => d.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<User>()
            .HasMany(o => o.OwnedRestaurants)
            .WithOne(r => r.Owner)
            .HasForeignKey(r => r.OwnerId);
    }
}

 