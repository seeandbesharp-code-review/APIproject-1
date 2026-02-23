using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Repository;
using TestProject;

namespace TestProject1
{
    public class ProductRepositoryIntegrationTest : IClassFixture<DatabaseFixture>,IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly dbSHOPContext _dbContext;
        private readonly ProductRepository _productRepository;
        public ProductRepositoryIntegrationTest()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context; _productRepository = new ProductRepository(_dbContext);
        }

        [Fact]
        public async Task GetProducts_WhenDataExists_ReturnsAllProductsWithCategory()
        {
            // Arrange
            var category = new Category { CategoryName = "TestCategory" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var testProducts = new List<Product>
            {
                new Product { ProductName = "Product1", Price = 10, CategoryId = category.CategoryId, Description = "Desc1" },
                new Product { ProductName = "Product2", Price = 20, CategoryId = category.CategoryId, Description = "Desc2" }
            };
            await _dbContext.Products.AddRangeAsync(testProducts);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _productRepository.GetProducts(null, null, null, null, null, null, null);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testProducts.Count, result.TotalCount);
            Assert.All(result.Items, p => Assert.NotNull(p.Category));
            foreach (var product in testProducts)
            {
                Assert.Contains(result.Items, p => p.ProductName == product.ProductName && p.Category != null);
            }
        }

        [Fact]
        public async Task GetProducts_ReturnsEmpty_WhenNoDataExists()
        {
            // Arrange
            // Act
            var result = await _productRepository.GetProducts(null, null, null, null, null, null, null);
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }
        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenIdExists()
        {
            // Arrange
            var category = new Category { CategoryName = "General" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var product = new Product
            {
                ProductName = "Test Product",
                Price = 100,
                Description = "Test",
                CategoryId = category.CategoryId
            };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProductById(product.ProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.ProductId, result.ProductId);
            Assert.Equal("Test Product", result.ProductName);
            Assert.Equal(100, result.Price);
        }

        [Fact]
        public async Task GetProductById_ReturnsNull_WhenIdDoesNotExist()
        {
            // Act
            var result = await _productRepository.GetProductById(999); // ID שלא קיים

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }
    }
}
