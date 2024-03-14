using dotnet_api_starter.Logging;
using dotnet_api_starter.Services.Abstraction;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.SetLogging();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
RegisterTransientServices(builder);
RegisterScopedServices(builder);
var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}

void RegisterTransientServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.Scan(scan => scan
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


void RegisterScopedServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.Scan(scan => scan
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