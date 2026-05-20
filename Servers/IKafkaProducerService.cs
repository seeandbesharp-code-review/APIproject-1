using DTOs;

namespace Servers
{
    public interface IKafkaProducerService
    {
        Task PublishOrderCreatedAsync(OrderDTO order);
    }
}
