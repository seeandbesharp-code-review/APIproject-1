using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entitys;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Repository;
using MailKit.Search;
using Moq.EntityFrameworkCore;

namespace TestProject1
{
    public class ProductRepositoryUnitTest
    {
        private List<Product> GetTestProducts()
        {
            var category = new Category { CategoryId = 1, CategoryName = "TestCategory" };
            return new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Product1", Price = 10, Category = category, CategoryId = 1, Description = "Desc1" },
                new Product { ProductId = 2, ProductName = "Product2", Price = 20, Category = category, CategoryId = 1, Description = "Desc2" }
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProductsWithCategory()
        {
            // Arrange
            var products = GetTestProducts();

            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var productRepository = new ProductRepository(mockContext.Object);

            // Act
            var result = await productRepository.GetProducts(null, null, null, null, null, null, null);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.All(result.Items, p => Assert.NotNull(p.Category));
        }

        [Fact]
        public async Task GetProducts_ReturnsEmptyList_WhenNoProducts()
        {
            // Arrange
            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(new List<Product>());

            var productRepository = new ProductRepository(mockContext.Object);

            // Act
            var result = await productRepository.GetProducts(null, null, null, null, null, null, null);

            // Assert
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task GetProductById_ProductExists_ReturnsProduct()
        {
            // Arrange
            var productId = 10;
            var expectedProduct = new Product { ProductId = productId, ProductName = "Gaming Chair", Price = 1200 };
            var products = new List<Product> { expectedProduct };

            var mockContext = new Mock<dbSHOPContext>();

            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            mockContext.Setup(x => x.Products.FindAsync(productId))
                       .ReturnsAsync(expectedProduct);

            var repository = new ProductRepository(mockContext.Object);

            // Act
            var result = await repository.GetProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.ProductId);
        }

        [Fact]
        public async Task GetProductById_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            var products = new List<Product>
           {
               new Product { ProductId = 1, ProductName = "Laptop" }
           };

            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            mockContext.Setup(x => x.Products.FindAsync(99))
                       .ReturnsAsync((Product)null);

            var repository = new ProductRepository(mockContext.Object);

            // Act
            var result = await repository.GetProductById(99);

            // Assert
            Assert.Null(result);
        }
    }
}
