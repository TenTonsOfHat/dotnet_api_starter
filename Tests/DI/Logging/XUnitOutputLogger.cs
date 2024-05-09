using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.

namespace Tests.DI.Logging;

/// <summary>
///     A logging provider copied and customized from Xunit.DependencyInjection.Logging
/// </summary>
public class XUnitOutputLogger(ITestOutputHelperAccessor accessor, string categoryName, Func<string, LogLevel, bool> filter)
    : ILogger
{
    private const string LogLevelPadding = ": ";
    public static bool IncludeContext = false;

    [ThreadStatic]
    private static StringBuilder _logBuilder;

    private readonly Func<string, LogLevel, bool> _filter = filter ?? throw new ArgumentNullException(nameof(filter));

    static XUnitOutputLogger()
    {
        //var logLevelString = GetLogLevelString(LogLevel.Information);
        //var messagePadding = new string(' ', logLevelString.Length + LogLevelPadding.Length);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _filter(categoryName, logLevel);
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!_filter(categoryName, logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = formatter(state, exception);

        if (!string.IsNullOrEmpty(message) || exception != null)
        {
            WriteMessage(logLevel, categoryName, eventId.Id, message, exception);
        }
    }

    public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
    {
        var logBuilder = _logBuilder;
        _logBuilder = null;

        logBuilder ??= new StringBuilder();

        var logLevelString = "";

        if (IncludeContext)
        {
            logLevelString = GetLogLevelString(logLevel);
            // category and event id
            logBuilder.Append(LogLevelPadding);
            logBuilder.Append(logName);
            logBuilder.Append("[");
            logBuilder.Append(eventId);
            logBuilder.Append("]: ");
        }


        if (!string.IsNullOrEmpty(message))
        {
            // message
            logBuilder.Append(message);
        }

        if (exception != null)
        {
            logBuilder.AppendLine(exception.ToString());
        }

        if (logBuilder.Length > 0)
        {
            if (!string.IsNullOrEmpty(logLevelString))
            {
                logBuilder.Insert(0, logLevelString);
            }

            try
            {
                accessor.Output?.WriteLine(logBuilder.ToString().TrimEnd());
            }
            catch (InvalidOperationException) //There is no currently active test case.
            {
                // ignored
            }
        }

        logBuilder.Clear();

        if (logBuilder.Capacity > 1024)
        {
            logBuilder.Capacity = 1024;
        }

        _logBuilder = logBuilder;
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }

    private class NullScope : IDisposable
    {
        private NullScope()
        {
        }

        public static NullScope Instance { get; } = new();

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}