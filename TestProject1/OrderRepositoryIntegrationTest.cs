using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Microsoft.EntityFrameworkCore;
using Repository;
using TestProject;

namespace TestProject1
{

    public class OrderRepositoryIntegrationTest : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly dbSHOPContext _dbContext;
        private readonly OrderRepository _orderRepository;

        private Order GetOrderTest()
        {
            return new Order
            {
                UserId = 1,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 76,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1 },
                }
            };
        }

        public OrderRepositoryIntegrationTest()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _orderRepository = new OrderRepository(_dbContext);
        }

        [Fact]
        public async Task AddOrder_PersistsOrderAndItemsToDatabase()
        {
            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "password!@#" };
            var category = new Category { CategoryName = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var product = new Product { ProductName = "TestProduct", Price = 50, Category = category };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            var newOrder = new Order
            {
                UserId = user.UserId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 100,
                OrderItems = new List<OrderItem>
          {
              new OrderItem { ProductId = product.ProductId, Quantity = 2 }
          }
            };
            // Act           
            var savedOrder = await _orderRepository.AddOrder(newOrder);
            // Assert
            Assert.NotEqual(0, savedOrder.OrderId);
            Assert.NotNull(savedOrder);
            Assert.Equal(newOrder.OrderId, savedOrder.OrderId);
            Assert.Equal(1, savedOrder.OrderItems.Count);
            Assert.Equal(product.ProductId, savedOrder.OrderItems.First().ProductId);
            Assert.Equal(2, savedOrder.OrderItems.First().Quantity);
            var dbOrder = await _dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == savedOrder.OrderId);
        }







        [Fact]
        public async Task GetOrderById_WhenOrderExists_ReturnsOrderWithItems()
        {
            // Arrange

            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "password!@#" };
            var category = new Category { CategoryName = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var product = new Product { ProductName = "TestProduct", Price = 50, Category = category };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var order = GetOrderTest();


            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetOrderById(order.OrderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
            Assert.NotNull(result.OrderItems);
            Assert.Equal(1, result.OrderItems.Count);
        }

        [Fact]
        public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _orderRepository.GetOrderById(9999);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }
    }
}
