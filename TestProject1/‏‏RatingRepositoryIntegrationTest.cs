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

    public class RatingRepositoryIntegrationTest : IClassFixture<DatabaseFixture>,IDisposable
    {
        private readonly dbSHOPContext _dbContext;
        private readonly RatingRepository _ratingRepository;

        private void ClearDatabase()
        {
            _dbContext.OrderItems.RemoveRange(_dbContext.OrderItems);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
        }

        public RatingRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _ratingRepository = new RatingRepository(_dbContext);
            ClearDatabase();
        }

        [Fact]
        public async Task AddRating_Integration_SavesToDatabase()
        {
            // Arrange
            var rating = new Rating
            {
                Host = "127.0.0.1",
                Method = "GET",
                Path = "/api/Categories",
                UserAgent = "Mozilla/5.0",
                Referer= "https://localhost:44324/index.html"

            };

            // Act
            var result = await _ratingRepository.AddRating(rating);

            // Assert
            Assert.NotEqual(0, result.RatingId);

            _dbContext.ChangeTracker.Clear();
            var saved = await _dbContext.Ratings.FindAsync(result.RatingId);

            // Assert
            Assert.NotNull(saved);
            Assert.Equal("GET", saved.Method.Trim());
            Assert.Equal("127.0.0.1", saved.Host.Trim());
        }

        public void Dispose()
        {
            ClearDatabase();
        }
    }
}
