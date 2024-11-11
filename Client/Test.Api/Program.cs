using Microsoft.Extensions.Configuration;
using Test.Api.Interceptors;
using Test.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddGrpcClient<TestProtoService.TestProtoServiceClient>(o =>
    {
        o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:MemberServiceUrl"));
    })
    .AddInterceptor<ClientLoggingInterceptor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
