using Customer.GRPC.Services;
using Customer.Infrastructure;
using Customer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

InfrastructureServiceRegistration.AddInfrastructureServices(builder.Services, builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddGrpc();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CustomerDatabaseContext>();
    var logger = services.GetRequiredService<ILogger<CustomerDatabaseContextSeed>>();
    var configuration = services.GetRequiredService<IConfiguration>(); 

    try
    {
        await context.Database.MigrateAsync();
        
        if (configuration.GetValue<bool>("RunMigrations"))
        {
            // Seed the "Customer API" database
            await CustomerDatabaseContextSeed.SeedDataAsync(context, logger);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"An error occurred while migrating or seeding the \"Customer.API\" database.");
    }
}

// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }

app.UseRouting();

app.MapGrpcService<CustomerService>();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Customer gRPC service.");
});

app.Run();