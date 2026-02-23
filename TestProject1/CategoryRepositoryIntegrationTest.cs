using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Repository;
using TestProject;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace TestProject1
{
    public class CategoryRepositoryIntegrationTest : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly dbSHOPContext _dbContext;
        private readonly CategoriesRepository _categoryRepository;
        public CategoryRepositoryIntegrationTest()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _categoryRepository = new CategoriesRepository(_dbContext);
        }


        [Fact]
        public async Task GetCategories_ReturnsAllCategories_whenDataExsists()
        {

            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryName = "Electronics" },
                new Category { CategoryName = "Books" },
                new Category { CategoryName = "Clothing" }
            };

            _dbContext.Categories.AddRange(categories);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.CategoryName == "Electronics");
            Assert.Contains(result, c => c.CategoryName == "Books");
            Assert.Contains(result, c => c.CategoryName == "Clothing");
        }
        [Fact]
        public async Task GetCategories_ReturnsEmpty_WhenNoDataExists()
        {
            // Arrange


            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

    }
}
