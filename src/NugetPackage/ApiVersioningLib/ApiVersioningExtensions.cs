namespace ApiVersioningLib;

public class ApiVersioning
    {
    private readonly Dictionary<string, string> _serviceVersions = new();

    public void AddService(string serviceName, string version)
    {
        _serviceVersions[serviceName] = version;
    }

    public string GetServiceVersion(string serviceName)
    {
        return (_serviceVersions.TryGetValue(serviceName, out var version) ? version : null)!;
    }

    public Dictionary<string, string> GetAllServiceVersions()
    {
        return new Dictionary<string, string>(_serviceVersions);
    }
    }