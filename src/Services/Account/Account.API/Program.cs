using Account.Infrastructure;
using Account.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

InfrastructureServiceRegistration.AddInfrastructureServices(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AccountDatabaseContext>();
    var logger = services.GetRequiredService<ILogger<AccountDatabaseContextSeed>>();

    try
    {
        await context.Database.MigrateAsync();
        
        // Seed the database
        await AccountDatabaseContextSeed.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account.API v1"));
}

app.UseHttpsRedirection();

app.Run();

