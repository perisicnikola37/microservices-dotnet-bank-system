using Account.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Test;

public abstract class BaseTest : IDisposable
    {
    protected const string ApiNamespace = "Account.API";  
    protected const string ApplicationNamespace = "Account.Application";
    protected const string DomainNamespace = "Account.Domain";
    protected const string InfrastructureNamespace = "Account.Infrastructure";
    protected readonly AccountDatabaseContext Context;

    protected BaseTest()
    {
        var serviceProvider = TestDatabaseFactory.CreateServiceProvider();

        var scope = serviceProvider.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<AccountDatabaseContext>();
    }

    protected static Assembly[] LoadAssemblies()
    {
        return
        [
            Assembly.Load(ApiNamespace),
            Assembly.Load(ApplicationNamespace),
            Assembly.Load(DomainNamespace),
            Assembly.Load(InfrastructureNamespace)
        ];
    }

    public void Dispose()
    {
        // Clean up the in-memory database after the tests
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
    }