using ApiVersioningLib.Exceptions;
using ApiVersioningLib.Helpers;
using ApiVersioningLib.Messages;

namespace ApiVersioningLib;

public class ApiVersioning
    {
    private readonly Dictionary<string, string> _serviceVersions = new();

    /// <summary>
    ///     Adds or updates the version of a service.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <param name="version">The version of the service.</param>
    /// <exception cref="InvalidVersionFormatException">Thrown when version format is invalid.</exception>
    /// <exception cref="ServiceVersionAlreadySetException">
    ///     Thrown if the version for the service already exists and is
    ///     different.
    /// </exception>
    public void AddService(string serviceName, string version)
    {
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException(ApiVersioningMessages.VersionCannotBeNullOrEmpty, nameof(version));

        if (!Utils.IsValidVersion(version)) throw new InvalidVersionFormatException(version);

        if (_serviceVersions.TryGetValue(serviceName, out var existingVersion) && existingVersion != version)
            throw new ServiceVersionAlreadySetException(serviceName, existingVersion);

        _serviceVersions[serviceName] = version;
    }

    /// <summary>
    ///     Retrieves the version of a specific service.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The version of the service.</returns>
    /// <exception cref="ServiceVersionNotFoundException">Thrown when the service version is not found.</exception>
    public string GetServiceVersion(string serviceName)
    {
        if (!_serviceVersions.TryGetValue(serviceName, out var version))
            throw new ServiceVersionNotFoundException(serviceName);
        return version;
    }

    /// <summary>
    ///     Retrieves the version of all services.
    /// </summary>
    /// <returns>All services versions.</returns>
    public Dictionary<string, string> GetAllServiceVersions()
    {
        return new Dictionary<string, string>(_serviceVersions);
    }
    }