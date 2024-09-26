using Account.Application.Contracts;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Infrastructure;

public abstract class InfrastructureServiceRegistration
    {
    public static void AddInfrastructureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AccountDatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AccountConnectionString")));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<IAccountRepository, AccountRepository>();
    }
    }