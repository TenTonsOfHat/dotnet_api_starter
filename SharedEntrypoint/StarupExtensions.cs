using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction;
using Services.Refit;

namespace SharedEntrypoint;

public static class StartupExtensions
{
    public static IServiceCollection RegisterAll(this IServiceCollection services)
    {
        return services.RegisterScopedServices()
            .RegisterTransientServices()
            .ConfigureRefitClients(new Uri("https://petstore3.swagger.io/api/v3"));
    }


    public static IServiceCollection RegisterTransientServices(this IServiceCollection services)
    {
        return services.Scan(scan => scan
            // We start out with all types in the assembly of ITransientService
            .FromAssemblyOf<ITransientService>()
            // AddClasses starts out with all public, non-abstract types in this assembly.
            // These types are then filtered by the delegate passed to the method.
            // In this case, we filter out only the classes that are assignable to ITransientService.
            .AddClasses(classes => classes.AssignableTo<ITransientService>())
            // We then specify what type we want to register these classes as.
            // In this case, we want to register the types as all of its implemented interfaces. 
            // So if a type implements 3 interfaces; A, B, C, we'd end up with three separate registrations.
            // Note: you can also us AsMatchingInterface, which registers the type with the first found matching interface name.  (e.g. ClassName is matched to IClassName).
            .AsImplementedInterfaces()
            // And lastly, we specify the lifetime of these registrations.
            // Transient means every service that uses this interface gets its own instance
            .WithTransientLifetime()
        );
    }


    public static IServiceCollection RegisterScopedServices(this IServiceCollection services)
    {
        return services.Scan(scan => scan
            // We start out with all types in the assembly of ITransientService
            .FromAssemblyOf<IScopedService>()
            // AddClasses starts out with all public, non-abstract types in this assembly.
            // These types are then filtered by the delegate passed to the method.
            // In this case, we filter out only the classes that are assignable to ITransientService.
            .AddClasses(classes => classes.AssignableTo<ITransientService>())
            // We then specify what type we want to register these classes as.
            // In this case, we want to register the types as all of its implemented interfaces. 
            // So if a type implements 3 interfaces; A, B, C, we'd end up with three separate registrations.
            // Note: you can also us AsMatchingInterface, which registers the type with the first found matching interface name.  (e.g. ClassName is matched to IClassName).
            .AsImplementedInterfaces()
            // And lastly, we specify the lifetime of these registrations.
            // Scoped means that you get one shared instance per request.
            .WithScopedLifetime()
        );
    }
}