//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using Moq;
//using DTOs;
//using Entitys;
//using Servers;
//using Repository;
//using AutoMapper;
//using Microsoft.Extensions.Logging;

//public class OrderServiceUnitTest
//{
//    [Fact]
//    public async Task AddOrder_WhenSumIsIncorrect_ShouldUpdateSum()
//    {
//        // Arrange
//        var mockProductRepo = new Mock<IProductRepository>();
//        var mockOrderRepo = new Mock<IOrderRepository>();
//        var mockMapper = new Mock<IMapper>();
//        var mockLogger = new Mock<ILogger<OrdersService>>();

//        mockProductRepo.Setup(r => r.GetProductById(3))
//                       .ReturnsAsync(new Product { ProductId = 3, Price = 100 });

//        var today = DateOnly.FromDateTime(DateTime.Now);
//        var orderItems = new List<OrderItemDTO> { new OrderItemDTO(3, 2) };

//        var inputOrderDto = new OrderDTO(today, 0, 50.0, orderItems, 1);

//        var orderEntity = new Order { OrderSum = 50 };
//        mockMapper.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(orderEntity);
//        mockOrderRepo.Setup(r => r.AddOrder(It.IsAny<Order>())).ReturnsAsync(orderEntity);

//        // 1. הגדרת המיפוי מ-DTO ל-Entity
//        mockMapper.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>()))
//                   .Returns((OrderDTO dto) => new Order
//                   {
//                       UserId = dto.userId,
//                       OrderSum = dto.OrderSum 
//                   });

//        // 2. הגדרת ה-Repository
//        mockOrderRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
//                     .ReturnsAsync((Order o) => o);

//        // 3. הגדרת המיפוי חזור ל-DTO
//        mockMapper.Setup(m => m.Map<Order, OrderDTO>(It.IsAny<Order>()))
//                   .Returns((Order src) => new OrderDTO(today, 0, src.OrderSum, orderItems, 1));
//        var orderService = new OrdersService(
//            mockOrderRepo.Object,
//            mockMapper.Object,
//            mockProductRepo.Object,
//            mockLogger.Object
//        );

//        // Act
//        var result = await orderService.AddOrder(inputOrderDto);

//        // Assert
//        Assert.Equal(200, result.OrderSum);
//    }

//    [Fact]
//    public async Task AddOrder_WhenSumIsCorrect_ShouldReturnSameSumAndNotLogWarning()
//    {
//        // Arrange
//        var mockProductRepo = new Mock<IProductRepository>();
//        var mockOrderRepo = new Mock<IOrderRepository>();
//        var mockMapper = new Mock<IMapper>();
//        var mockLogger = new Mock<ILogger<OrdersService>>();

//        var today = DateOnly.FromDateTime(DateTime.Now);
//        var orderItems = new List<OrderItemDTO> { new OrderItemDTO(3, 2) };

//        mockProductRepo.Setup(r => r.GetProductById(3))
//                       .ReturnsAsync(new Product { ProductId = 3, Price = 100 });

//        var inputOrderDto = new OrderDTO(today, 0, 200.0, orderItems, 1);

//        mockMapper.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>()))
//                   .Returns((OrderDTO dto) => new Order { UserId = dto.userId, OrderSum = dto.OrderSum });

//        mockOrderRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
//                     .ReturnsAsync((Order o) => o);

//        mockMapper.Setup(m => m.Map<Order, OrderDTO>(It.IsAny<Order>()))
//                   .Returns((Order src) => new OrderDTO(today, 0, src.OrderSum, orderItems, 1));

//        var orderService = new OrdersService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockLogger.Object);

//        // Act
//        var result = await orderService.AddOrder(inputOrderDto);

//        // Assert
//        Assert.Equal(200, result.OrderSum);

//        // בדיקה שהלוגר מעולם לא הופעל 
//        mockLogger.Verify(l => l.Log(
//            LogLevel.Warning,
//            It.IsAny<EventId>(),
//            It.IsAny<It.IsAnyType>(),
//            null,
//            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
//    }
//}

using AutoMapper;
using DTOs;
using Entitys;
using Microsoft.Extensions.Logging;
using Moq;
using Repository;
using Servers;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class OrderServiceUnitTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<OrdersService>> _loggerMock;
        private readonly OrdersService _ordersService;

        public OrderServiceUnitTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OrdersService>>();

            _ordersService = new OrdersService(
                _orderRepositoryMock.Object,
                _mapperMock.Object,
                _productRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task AddOrder_SumIsCorrect_ReturnsOrderDto()
        {
            // Arrange - Happy Path
            var items = new List<OrderItemDTO> { new OrderItemDTO( 1, 2) }; // בהנחה שהסדר הוא ID, ProductId, Quantity
            OrderDTO orderDto = new OrderDTO(DateOnly.FromDateTime(DateTime.Now), 0, 100, items,1);

            Product product = new Product { ProductId = 1, Price = 50 };
            Order orderEntity = new Order { OrderSum = 100 };
            OrderDTO savedOrderDto = new OrderDTO(DateOnly.FromDateTime(DateTime.Now), 1, 100, items, 1);


            _productRepositoryMock.Setup(r => r.GetProductById(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<OrderDTO, Order>(It.IsAny<OrderDTO>())).Returns(orderEntity);
            _orderRepositoryMock.Setup(r => r.AddOrder(orderEntity)).ReturnsAsync(orderEntity);
            _mapperMock.Setup(m => m.Map<Order, OrderDTO>(orderEntity)).Returns(savedOrderDto);

            // Act
            var result = await _ordersService.AddOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.OrderSum);

            // בדיקה שהלוגר מעולם לא הופעל 
            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task AddOrder_SumMismatch_CorrectsSumAndLogsWarning()
        {
            // Arrange - Unhappy Path
            var items = new List<OrderItemDTO> { new OrderItemDTO( 1, 2) };
            OrderDTO orderDto = new OrderDTO(DateOnly.FromDateTime(DateTime.Now), 0, 50, items, 1); 

            var product = new Product { ProductId = 1, Price = 60 }; 
            var orderEntity = new Order();
            OrderDTO savedOrderDto = new OrderDTO(DateOnly.FromDateTime(DateTime.Now), 1, 120, items, 1);

            _productRepositoryMock.Setup(r => r.GetProductById(1)).ReturnsAsync(product);

            // כאן אנחנו מוודאים שה-Mapper מקבל אובייקט שהסכום שלו כבר תוקן ל-120
            _mapperMock.Setup(m => m.Map<OrderDTO, Order>(It.Is<OrderDTO>(o => o.OrderSum == 120)))
                       .Returns(orderEntity);

            _orderRepositoryMock.Setup(r => r.AddOrder(orderEntity)).ReturnsAsync(orderEntity);
            _mapperMock.Setup(m => m.Map<Order, OrderDTO>(orderEntity)).Returns(savedOrderDto);

            // Act
            var result = await _ordersService.AddOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(120, result.OrderSum);

            // בדיקה שהלוגר אכן הופעל
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Order sum mismatch")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
