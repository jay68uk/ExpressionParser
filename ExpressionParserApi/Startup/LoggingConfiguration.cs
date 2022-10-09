using Serilog;
using Serilog.Core;

namespace ExpressionParserApi.Startup;

internal static class LoggingConfiguration
{
    internal static Logger ConfigureLogging()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}