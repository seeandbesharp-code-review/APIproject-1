namespace KafkaConsumer;

// Mirrors DTOs.OrderDTO — kept local so this project has zero dependencies on the main solution
public record OrderItemDTO(int ProductId, int Quantity);

public record OrderDTO(
    DateOnly OrderDate,
    int OrderId,
    double OrderSum,
    ICollection<OrderItemDTO> OrderItems,
    int userId);
