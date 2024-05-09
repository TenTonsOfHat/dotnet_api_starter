using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;

namespace Tests.DI.Logging;

/// <summary>
///     A logging provider copied and customized from Xunit.DependencyInjection.Logging
/// </summary>
public class XUnitLoggerProvider(ITestOutputHelperAccessor accessor, Func<string, LogLevel, bool> filter)
    : ILoggerProvider
{
    private readonly Func<string, LogLevel, bool> _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    private readonly ConcurrentDictionary<string, ILogger> _loggers = new();

    /// <summary>Log minLevel LogLevel.Information</summary>
    public XUnitLoggerProvider(ITestOutputHelperAccessor accessor) : this(accessor, (_, _) => true)
    {
    }

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new XUnitOutputLogger(accessor, name, _filter));
    }

    public static void Register(IServiceProvider provider)
    {
        provider.GetRequiredService<ILoggerFactory>().AddProvider(ActivatorUtilities.CreateInstance<XUnitLoggerProvider>(provider));
    }
}