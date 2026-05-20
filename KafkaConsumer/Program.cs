using Confluent.Kafka;
using KafkaConsumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Text.Json;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddNLog("nlog.config");
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<OrderConsumerService>();
    })
    .Build();

await host.RunAsync();


// ── Background consumer service ──────────────────────────────────────────────
public class OrderConsumerService : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;
    private readonly ILogger<OrderConsumerService> _logger;

    public OrderConsumerService(IConfiguration configuration, ILogger<OrderConsumerService> logger)
    {
        _logger = logger;
        _topic  = configuration["Kafka:OrderTopic"]!;

        string bootstrapServers = configuration["Kafka:BootstrapServers"]!;

        if (string.IsNullOrWhiteSpace(bootstrapServers))
            throw new InvalidOperationException(
                "Kafka:BootstrapServers is empty. " +
                "Make sure DOTNET_ENVIRONMENT=Development is set and appsettings.Development.json is present.");

        _logger.LogInformation("Kafka consumer connecting to: {Servers}", bootstrapServers);

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId          = configuration["Kafka:ConsumerGroup"],
            AutoOffsetReset  = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);

    private void ConsumeLoop(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        _logger.LogInformation("Consumer started — listening on topic: {Topic}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<string, string> result = _consumer.Consume(stoppingToken);
                OrderDTO? order = JsonSerializer.Deserialize<OrderDTO>(result.Message.Value);

                if (order is null) continue;

                _logger.LogInformation(
                    "ORDER RECEIVED ← Partition: {Partition} | Offset: {Offset}\n" +
                    "  OrderId  : {OrderId}\n"  +
                    "  UserId   : {UserId}\n"   +
                    "  Date     : {Date}\n"     +
                    "  Total    : {Sum}\n"      +
                    "  Items    : {Items}",
                    result.Partition.Value,
                    result.Offset.Value,
                    order.OrderId,
                    order.userId,
                    order.OrderDate,
                    order.OrderSum,
                    JsonSerializer.Serialize(order.OrderItems));
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message");
            }
        }

        _consumer.Close();
    }

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
    }
}
