using Account.API.Middlewares;
using Account.Application;
using Account.Infrastructure;
using Account.Infrastructure.Persistence;
using Common.Logging;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
InfrastructureServiceRegistration.AddInfrastructureServices(builder.Services, builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

var accountApiVersion = builder.Configuration["APIVersion"];

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(s =>
{
    // Define the API versioning
    s.SwaggerDoc($"v{accountApiVersion}", new OpenApiInfo
    {
        Title = "Account service",
        Version = $"v{accountApiVersion}", 
        Description = "Service for managing customer accounts.",
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

builder.Services.AddHealthChecks()
    .AddRabbitMQ(builder.Configuration["EventBusSettings:HostAddress"]!, name: "account-transaction-rabbitmq_bus")
    .AddDbContextCheck<AccountDatabaseContext>();

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddMassTransit(conf =>
{
    conf.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]!);
    });
});

builder.Services.Configure<MassTransitHostOptions>(conf =>
{
    conf.WaitUntilStarted = true;
    conf.StartTimeout = TimeSpan.FromSeconds(30);
    conf.StopTimeout = TimeSpan.FromMinutes(1);
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AccountDatabaseContext>();
    var logger = services.GetRequiredService<ILogger<AccountDatabaseContextSeed>>();
    var configuration = services.GetRequiredService<IConfiguration>(); 

    try
    {
        await context.Database.MigrateAsync();
        
        if (configuration.GetValue<bool>("RunMigrations"))
        {
            // Seed the "Account API" database
            await AccountDatabaseContextSeed.SeedDataAsync(context, logger);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"An error occurred while migrating or seeding the \"Account.API\" database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/v{accountApiVersion}/swagger.json", $"Account.API v{accountApiVersion}"));
}

// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }

    await next();
});

app.MapHealthChecks("/health-check", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

