using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedEntrypoint;
using Tests.DI.Logging;
using Xunit.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Tests;

public class Startup
{
    
    public static IHostBuilder HostBuilder { get; set; }
    public static IWebHostBuilder WebApplicationBuilder { get; set; }
    
    // ReSharper disable once UnusedMember.Global
    /// <summary>
    ///     This method is called via reflection by the logic here: https://github.com/pengweiqhca/Xunit.DependencyInjection
    ///     Note: This class must have this name and be present in the top level namespace to be picked up
    /// </summary>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder()
    {
        
        
        HostBuilder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.CaptureStartupErrors(true);
                webBuilder.ConfigureLogging(x => x.SetMinimumLevel(LogLevel.Debug));
            });
        
        
        
      
        return HostBuilder;
    }

 

    ///// <summary>
    ///// This method is called via reflection by the logic here: https://github.com/pengweiqhca/Xunit.DependencyInjection
    ///// </summary>
    ///// <returns></returns>
    // ReSharper disable once UnusedMember.Global
    public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
    {
        loggerFactory.AddProvider(new XUnitLoggerProvider(accessor));
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterAll();
    }
    
}