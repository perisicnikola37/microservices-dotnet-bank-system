using Account.Application.Contracts.Persistence;
using Account.Application.Features.Accounts.Commands.CreateAccount;
using Account.Application.Mappings;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Test;

public class TestDatabaseFactory
    {
    public static ServiceProvider CreateServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<AccountDatabaseContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"))
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddMediatR(cfg => 
                cfg.RegisterServicesFromAssemblies(
                    Assembly.Load("Account.Application")
                ))
            .AddAutoMapper(typeof(MappingProfile))
            .AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            })
            .AddLogging()
            .BuildServiceProvider();

        return serviceProvider;
    }
    }