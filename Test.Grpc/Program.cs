using Google.Protobuf.WellKnownTypes;
using Test.Grpc.Interceptors;
using Test.Grpc.Services;
using Test.Infrastructure;
using Test.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<ServerLoggerInterceptor>>();
builder.Services.AddSingleton(typeof(ILogger),logger);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ServerLoggerInterceptor>();
});

//builder.Services.AddSingleton<ServerLoggerInterceptor>();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddTransient<DbInitializer>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);


var app = builder.Build();
//This line of code enables gRPC-Web support in the ASP.NET Core application. gRPC-Web is a protocol that allows gRPC services to be accessed over HTTP.
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

// Configure the HTTP request pipeline.
app.MapGrpcService<TestService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

var initialiser = services.GetRequiredService<DbInitializer>();
await initialiser.Run();


app.Run();
