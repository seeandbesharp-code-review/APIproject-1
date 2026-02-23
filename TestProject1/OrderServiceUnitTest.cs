using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DTOs;
using Entitys;
using Servers;
using Repository;
using AutoMapper;
using Microsoft.Extensions.Logging;

public class OrderServiceUnitTest
{
        [Fact]
    public async Task AddOrder_WhenSumIsIncorrect_ShouldUpdateSum()
    {
        // Arrange
        var mockProductRepo = new Mock<IProductRepository>();
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<OrdersService>>();

        mockProductRepo.Setup(r => r.GetProductById(3))
                       .ReturnsAsync(new Product { ProductId = 3, Price = 100 });

        var today = DateOnly.FromDateTime(DateTime.Now);
        var orderItems = new List<OrderItemDTO> { new OrderItemDTO(3, 2) };

        var inputOrderDto = new OrderDTO(today, 0, 50.0, orderItems, 1);

        var orderEntity = new Order { OrderSum = 50 };
        mockMapper.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(orderEntity);
        mockOrderRepo.Setup(r => r.AddOrder(It.IsAny<Order>())).ReturnsAsync(orderEntity);

        // 1. הגדרת המיפוי מ-DTO ל-Entity
        mockMapper.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>()))
                   .Returns((OrderDTO dto) => new Order
                   {
                       UserId = dto.userId,
                       OrderSum = dto.OrderSum 
                   });

        // 2. הגדרת ה-Repository
        mockOrderRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
                     .ReturnsAsync((Order o) => o);

        // 3. הגדרת המיפוי חזור ל-DTO
        mockMapper.Setup(m => m.Map<Order, OrderDTO>(It.IsAny<Order>()))
                   .Returns((Order src) => new OrderDTO(today, 0, src.OrderSum, orderItems, 1));
        var orderService = new OrdersService(
            mockOrderRepo.Object,
            mockMapper.Object,
            mockProductRepo.Object,
            mockLogger.Object
        );

        // Act
        var result = await orderService.AddOrder(inputOrderDto);

        // Assert
        Assert.Equal(200, result.OrderSum);
    }

    [Fact]
    public async Task AddOrder_WhenSumIsCorrect_ShouldReturnSameSumAndNotLogWarning()
    {
        // Arrange
        var mockProductRepo = new Mock<IProductRepository>();
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<OrdersService>>();

        var today = DateOnly.FromDateTime(DateTime.Now);
        var orderItems = new List<OrderItemDTO> { new OrderItemDTO(3, 2) };

        // מוצר עולה 100 (100 * 2 = 200)
        mockProductRepo.Setup(r => r.GetProductById(3))
                       .ReturnsAsync(new Product { ProductId = 3, Price = 100 });

        // הלקוח שלח 200 (סכום תקין)
        var inputOrderDto = new OrderDTO(today, 0, 200.0, orderItems, 1);

        mockMapper.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>()))
                   .Returns((OrderDTO dto) => new Order { UserId = dto.userId, OrderSum = dto.OrderSum });

        mockOrderRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
                     .ReturnsAsync((Order o) => o);

        mockMapper.Setup(m => m.Map<Order, OrderDTO>(It.IsAny<Order>()))
                   .Returns((Order src) => new OrderDTO(today, 0, src.OrderSum, orderItems, 1));

        var orderService = new OrdersService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockLogger.Object);

        // Act
        var result = await orderService.AddOrder(inputOrderDto);

        // Assert
        Assert.Equal(200, result.OrderSum);

        // בדיקה שהלוגר מעולם לא הופעל (כי הסכום היה תקין)
        mockLogger.Verify(l => l.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
    }
}
