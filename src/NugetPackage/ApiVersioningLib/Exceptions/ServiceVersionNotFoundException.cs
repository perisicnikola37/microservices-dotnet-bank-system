using ApiVersioningLib.Messages;

namespace ApiVersioningLib.Exceptions;

public class ServiceVersionNotFoundException(string serviceName)
    : KeyNotFoundException(string.Format(ApiVersioningMessages.ServiceVersionNotFound, serviceName));