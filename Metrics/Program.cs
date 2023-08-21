using System.Text.Json.Serialization;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Metrics;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton(new TransactionMetrics());

        builder.Services.AddOpenTelemetry()
            .WithTracing(traceProviderBuilder => traceProviderBuilder
                .AddOtlpExporter(opts =>
                {
                    opts.Endpoint = new Uri("http://localhost:4318/v1/traces");
                }))
            // .WithMetrics(meterProviderBuilder => meterProviderBuilder
            //     .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("TransactionService.WebApi"))
            //     .AddMeter(TransactionMetrics.MetricName)
            //     .AddOtlpExporter(opts =>
            //     {
            //         opts.Endpoint = new Uri("http://localhost:4318/v1/traces");
            //     }))
            ;

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        
        app.MapControllers();

        app.Run();
    }
}
