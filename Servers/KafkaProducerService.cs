using Confluent.Kafka;
using DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Servers
{
    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            _topic = configuration["Kafka:OrderTopic"]!;

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            // ProducerBuilder is the official Confluent class for building a Kafka producer
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishOrderCreatedAsync(OrderDTO order)
        {
            var message = new Message<string, string>
            {
                Key   = order.userId.ToString(),   // partition key — all orders of same user go to same partition
                Value = JsonSerializer.Serialize(order)
            };

            DeliveryResult<string, string> result = await _producer.ProduceAsync(_topic, message);

            _logger.LogInformation(
                "Order event published → Topic: {Topic} | Partition: {Partition} | Offset: {Offset} | UserId: {UserId} | OrderId: {OrderId} | Sum: {Sum}",
                result.Topic, result.Partition.Value, result.Offset.Value,
                order.userId, order.OrderId, order.OrderSum);
        }

        public void Dispose() => _producer?.Dispose();
    }
}
