using Serilog;

namespace dotnet_api_starter.Logging.Registrations;

public interface ILogRegistration
{
    LoggerConfiguration EnrichLog(LoggerConfiguration logger, IConfiguration configuration);

    LoggerConfiguration AddWriteTo(LoggerConfiguration logger, IConfiguration configuration);
}