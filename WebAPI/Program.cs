using Serilog;
using Services.Abstraction;
using SharedEntrypoint;
using SharedEntrypoint.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.SetLogging();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterScopedServices().RegisterTransientServices();


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
