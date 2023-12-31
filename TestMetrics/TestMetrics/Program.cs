using System.Text.Json.Serialization;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace TestMetrics;

public static class Program
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
            .WithMetrics(meterProviderBuilder => meterProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("TransactionService.WebApi"))
                .AddMeter(TransactionMetrics.MetricName)
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddOtlpExporter(opts =>
                {
                    opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                    opts.Endpoint = new Uri("http://localhost:4318/v1/metrics");
                }));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
