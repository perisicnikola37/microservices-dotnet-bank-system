using System.Reflection;

namespace Account.Test;

public abstract class BaseTest
    {
    protected const string ApiNamespace = "Account.API";  
    protected const string ApplicationNamespace = "Account.Application";
    protected const string DomainNamespace = "Account.Domain";
    protected const string InfrastructureNamespace = "Account.Infrastructure";

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
    }