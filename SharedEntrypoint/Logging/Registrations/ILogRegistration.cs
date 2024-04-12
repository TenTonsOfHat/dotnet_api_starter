using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedEntrypoint.Logging.Registrations;

public interface ILogRegistration
{
    LoggerConfiguration EnrichLog(LoggerConfiguration logger, IConfiguration configuration);

    LoggerConfiguration AddWriteTo(LoggerConfiguration logger, IConfiguration configuration);
}