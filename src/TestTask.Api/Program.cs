using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TestTask.Api.Middlewares;
using TestTask.Core;
using TestTask.Infrastructure.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();
builder.Services.AddInfrastructureSQL(builder.Configuration);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.EnableAnnotations();
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TestTask API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestTask API V1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();

app.Run();
