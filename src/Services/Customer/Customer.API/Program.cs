using Customer.Application;
using Customer.Infrastructure;
using Customer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc($"v1.0.0", new OpenApiInfo
    {
        Title = "Account service",
        Version = "v1.0.0", 
        Description = "Service for managing customers.",
        Contact = new OpenApiContact
        {
            Name = "Nikola Perišić",
            Email = "nikola.perisic@vegait.rs",
            Url = new Uri("https://github.com/perisicnikola37")
        },
        License = new OpenApiLicense
        {
            Name = "LinkedIn",
            Url = new Uri("https://www.linkedin.com/in/perisicnikola37")
        }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
InfrastructureServiceRegistration.AddInfrastructureServices(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CustomerDatabaseContext>();
    var logger = services.GetRequiredService<ILogger<CustomerDatabaseContextSeed>>();

    try
    {
        await context.Database.MigrateAsync();
        
        // Seed the "Customer API" database
        await CustomerDatabaseContextSeed.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"An error occurred while migrating or seeding the \"Customer.API\" database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

