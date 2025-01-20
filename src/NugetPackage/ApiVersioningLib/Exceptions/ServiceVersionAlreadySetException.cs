using ApiVersioningLib.Messages;

namespace ApiVersioningLib.Exceptions;

public class ServiceVersionAlreadySetException(string serviceName, string existingVersion)
    : InvalidOperationException(string.Format(ApiVersioningMessages.ServiceVersionAlreadySet, serviceName,
        existingVersion));