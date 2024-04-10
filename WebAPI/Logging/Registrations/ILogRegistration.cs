using Serilog;

namespace api.Logging.Registrations;

public interface ILogRegistration
{
    LoggerConfiguration EnrichLog(LoggerConfiguration logger, IConfiguration configuration);

    LoggerConfiguration AddWriteTo(LoggerConfiguration logger, IConfiguration configuration);
}