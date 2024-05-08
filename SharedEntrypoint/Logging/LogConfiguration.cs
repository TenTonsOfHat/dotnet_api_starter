using System.Text.RegularExpressions;
using Cocona.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using SharedEntrypoint.Logging.Registrations;

namespace SharedEntrypoint.Logging;

public static class LogConfiguration
{
    public static ILogRegistration[] Registrations { get; } =
    {
        new ConsoleLogRegistration(),
    };

    public static WebApplicationBuilder SetLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        InitializeLogger(builder.Configuration);
        return builder;
    }

    public static CoconaAppBuilder SetLogging(this CoconaAppBuilder builder)
    {
        builder.Host.UseSerilog();
        InitializeLogger(builder.Configuration);
        return builder;
    }

    public static void InitializeLogger(IConfiguration configuration)
    {
        Log.Logger = BuildLoggerConfiguration(configuration, Registrations).CreateLogger();
    }

    private static LoggerConfiguration BuildLoggerConfiguration(IConfiguration appConfig, params ILogRegistration[] registrations)
    {
        if (registrations == null)
        {
            throw new ArgumentNullException(nameof(registrations));
        }

        if (registrations.Length == 0)
        {
            throw new ArgumentException("Value cannot be an empty collection.", nameof(registrations));
        }

        var logConfig = BuildLogConfiguration(appConfig);

        foreach (var registration in registrations)
        {
            logConfig = registration.EnrichLog(logConfig, appConfig);
        }

        foreach (var registration in registrations)
        {
            logConfig = registration.AddWriteTo(logConfig, appConfig);
        }

        return logConfig;
    }


    private static LoggerConfiguration BuildLogConfiguration(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration) // log levels are set in appsettings.json
            .Filter.ByExcluding(ShouldExcludeByProperty)
            .Enrich.FromLogContext()
            .Enrich.WithClientIp()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName();
    }


    private static readonly Regex PathExclusionRegex = new(".*swagger\\.json|.*index\\.html", RegexOptions.Compiled);
    private static readonly string[] PropertiesToCheck = new[] { "Path", "RequestPath" };

    private static bool ShouldExcludeByProperty(LogEvent c)
    {
        foreach (var prop in PropertiesToCheck)
        {
            if (c.Properties.TryGetValue(prop, out var value) && PathExclusionRegex.IsMatch(value.ToString()))
            {
                return true;
            }
        }

        return false;
    }
}