using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Transaction.API;
using Transaction.API.Data;
using Transaction.API.Data.Interfaces;
using Transaction.API.EventBusConsumer;
using Transaction.API.Repositories;
using Transaction.API.Repositories.Interfaces;
using Transaction.API.Services;
using Transaction.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(conf =>
{
    conf.AddConsumer<AccountTransactionConsumer>();
    conf.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]!);
        cfg.ReceiveEndpoint(EventBusConstants.AccountTransactionQueue, c =>
        {
            c.ConfigureConsumer<AccountTransactionConsumer>(ctx);
        });
    });
});

builder.Services.Configure<MassTransitHostOptions>(conf =>
{
    conf.WaitUntilStarted = true;
    conf.StartTimeout = TimeSpan.FromSeconds(30);
    conf.StopTimeout = TimeSpan.FromMinutes(1);
});


builder.Services.AddScoped<ITransactionContext, TransactionDatabaseContext>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));

var transactionApiVersion = builder.Configuration["APIVersion"];

builder.Services.AddSwaggerGen(s =>
{
    // Define the API versioning
    s.SwaggerDoc($"v{transactionApiVersion}", new OpenApiInfo
    {
        Title = "Transaction service",
        Version = $"v{transactionApiVersion}", 
        Description = "Service for managing transactions.",
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
    .AddRabbitMQ(builder.Configuration["EventBusSettings:HostAddress"]!, name: "account-transaction-rabbitmqbus")
    .AddMongoDb(builder.Configuration["DatabaseSettings:ConnectionString"]!, "MongoDb Health", HealthStatus.Degraded);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ITransactionContext>();
    var configuration = services.GetRequiredService<IConfiguration>();

    if (configuration.GetValue<bool>("RunMigrations"))
    {
        Console.WriteLine("Running migrations...");
        TransactionDatabaseContextSeed.SeedData(context.Transactions);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction.API v1"));
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health-check", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();


