using ApiVersioningLib.Messages;

namespace ApiVersioningLib.Exceptions;

public class InvalidVersionFormatException(string version)
    : ArgumentException(string.Format(ApiVersioningMessages.InvalidVersionFormat, version));