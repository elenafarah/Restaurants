using Restaurants.Api.Extensions;
using Restaurants.Api.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;

try
{


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPresentation();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

/*
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .WriteTo.File("Logs/Restaurant-Api-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
        .WriteTo.Console(outputTemplate : "[{Timestamp: dd-MM-yyyy HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine}{Message:lj}{NewLine}{Exception}");
});
*/



var app = builder.Build();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();

var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

await seeder.Seed();

await IdentitySeeder.SeedAsync(app.Services);

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapGroup("api/identity").
    WithTags("Identity")
    .MapIdentityApi<User>();

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup Failed!");
}
finally
{
    Log.CloseAndFlush();
}
public partial class Program {}