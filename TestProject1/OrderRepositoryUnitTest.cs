using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;

namespace TestProject1
{
    public class OrderRepositoryUnitTest
    {
        [Fact]
        public async Task GetOrderById_OrderExists_ReturnsOrderWithItems()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderSum=20,
                    UserId=1,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { OrderItemId = 10,ProductId=1,Quantity=1}
                    }
                }
            };

            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders);

            var Orderrepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await Orderrepository.GetOrderById(1);

            // Assert
            Assert.Equal(1, result.OrderId);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task GetOrderById_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orders = new List<Order>();
            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders);

            var Orderrepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await Orderrepository.GetOrderById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddOrder_ValidOrder_AddsAndSavesOrder()
        {
            // Arrange

            var order = new Order
            {
                UserId = 1,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 76,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1 },
                    new OrderItem { ProductId = 2, Quantity = 1 }
                }
            };

            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Orders).ReturnsDbSet(new List<Order>());

            var Orderrepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await Orderrepository.AddOrder(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);

            mockContext.Verify(c => c.AddAsync(order, default), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }


    }
}
