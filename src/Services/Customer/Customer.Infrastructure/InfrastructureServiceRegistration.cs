using Customer.Application.Contracts.Persistance;
using Customer.Infrastructure.Persistence;
using Customer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infrastructure;

public abstract class InfrastructureServiceRegistration
    {
    public static void AddInfrastructureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CustomerDatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CustomerConnectionString")));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
    }
    }