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
    public class UserRepositoryIntegrationTests : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly dbSHOPContext _dbContext;
        private readonly UserRepository _userRepository;
        public UserRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _userRepository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task GetUserById_ReturnsNull_WhenUserDontExist()
        {
            //Arange

            //Act
            var result = await _userRepository.GetUserById(999);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsUserWithOrders_WhenUserExists()
        {
            //Arange

            var user = new User { UserFirstName = "Alice", UserEmail = "alice@db.com", UserPassword = "alice@db.comFGH123" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _userRepository.GetUserById(user.UserId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserId, result.UserId);
            var fetchedUser = result.UserEmail == "alice@db.com";


        }

        [Fact]
        public async Task AddUser_ShouldAddOrderToDatabase()
        {
            // Arrange

            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "pasSSsword!@#" };

            // Act
            var result = await _userRepository.AddUser(user);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.UserId > 0);

            var userFromDb = await _dbContext.Users.FindAsync(result.UserId);
            Assert.NotNull(userFromDb);
        }

        [Fact]
        public async Task Login_ReturnsUser_WhenCredentialsMatch()
        {
            // Arrange
            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "pasSSsword!@#" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login("TestUser@.com", "pasSSsword!@#");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.UserId > 0);
        }

        [Fact]
        public async Task Login_ReturnsNull_WhenPasswordNotMatch()
        {
            // Arrange
            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "pasSSsword!@#" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login("TestUser@.com", "wrongPassword!@#");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_ReturnsNull_WhenEmailNotMatch()
        {
            // Arrange
            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "pasSSsword!@#" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login("wrongEmail@.com", "pasSSsword!@#");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUserInDatabase()
        {
            // Arrange
            var user = new User { UserFirstName = "OldName", UserEmail = "user@.com", UserPassword = "oldPassword!@#" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            user.UserFirstName = "NewName";
            await _userRepository.UpdateUser(user);

            // Assert
            var updatedUser = await _dbContext.Users.FindAsync(user.UserId);
            Assert.NotNull(updatedUser);
            Assert.Equal("NewName", updatedUser.UserFirstName);

        }

        public void Dispose()
        {
            _fixture.Dispose();
        }
    }
}
