namespace Common.Logging;

public static class SeriLogger
{
    public static ILogger CreateLogger<T>() => new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();
}
