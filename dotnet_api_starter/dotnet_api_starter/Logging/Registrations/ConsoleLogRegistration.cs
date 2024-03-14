using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace dotnet_api_starter.Logging.Registrations;

public class ConsoleLogRegistration : ILogRegistration
{
    public LoggerConfiguration EnrichLog(LoggerConfiguration logger, IConfiguration configuration) 
        => logger;

    public LoggerConfiguration AddWriteTo(LoggerConfiguration logger, IConfiguration configuration) 
        => logger.WriteTo.Console(theme: AnsiConsoleTheme.Code);
}