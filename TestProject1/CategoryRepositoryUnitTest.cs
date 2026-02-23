using Entitys;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;

namespace TestProject1
{
    public class CategoryRepositoryUnitTest
    {
        [Fact]
        
        public async Task GetCategory_ValidCredentials_ReturnsCategory()
        {
            // Arrange
            var category1 = new Category { CategoryName = "toys" };
            var category2 = new Category { CategoryName = "books" };

            var mockContext = new Mock<dbSHOPContext>();
            var categories = new List<Category>() { category1, category2 };
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);

            var categoryRepository = new CategoriesRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.Equal(categories, result);
        }

        [Fact]
        public async Task GetCategories_WhenNoCategoriesExist_ReturnsEmptyList()
        {
            // Arrange
            var categories = new List<Category>() {};
            var mockContext = new Mock<dbSHOPContext>();
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);

            var categoryRepository = new CategoriesRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);          // לא null
            Assert.Empty(result);            // אין פריטים = NOITEM
        }
    }
}

