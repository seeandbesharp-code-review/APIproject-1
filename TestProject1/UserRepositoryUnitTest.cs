using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Repository;
using Entitys;
using System.Collections.Generic;
using System.Linq;
using Moq.EntityFrameworkCore;

namespace TestProject1;

public class UserRepositoryUnitTest
{

    private List<User> GetTestUsers()
    {
        return new List<User>
        {
                new User
                {
                    UserId = 1,
                    UserFirstName = "Alice",
                    UserLastName = "Smith",
                    UserEmail = "alice@test.com",
                    UserPassword="alice@test.comSD%^67",
                    Orders = new List<Order> { new Order { OrderId = 101, OrderSum = 50.0 } }
                },
                new User
                {
                    UserId = 2,
                    UserFirstName = "Bob",
                    UserLastName="Johnson",
                    UserEmail = "bob@test.com",
                    UserPassword="bob@test.comSD%^67",
                    Orders = new List<Order> ()
                }
        };
    }

    [Fact]
    public async Task GetUserById_ReturnsUserWithOrders_WhenUserExists()
    {
        // Arrange
        var users = GetTestUsers();

        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(c => c.FindAsync<User>(1)).ReturnsAsync(users.First);
        var userRepository = new UserRepository(mockContext.Object);

        //Act
        var result = await userRepository.GetUserById(1);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Single(result.Orders);


    }

    [Fact]
    public async Task GetUserById_ReturnsNull_WhenUserNotExists()
    {
        // Arrange
        var users = GetTestUsers();
        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(c => c.FindAsync<User>(5)).ReturnsAsync((User)null);
        var userRepository = new UserRepository(mockContext.Object);

        // Act
        var result = await userRepository.GetUserById(1);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddUser_AddsUserAndReturnsUser()
    {
        // Arrange
        var users = GetTestUsers();

        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

        var userRepository = new UserRepository(mockContext.Object);


        //Act
        var result = await userRepository.AddUser(users[1]);

        //Assert
        mockContext.Verify(m => m.AddAsync(users[1], default), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        Assert.Equal(2, result.UserId);

    }

    [Fact]
    public async Task Login_ReturnsUser_WhenCredentialsMatch()
    {
        // Arrange
        var users =GetTestUsers();

        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        var userRepository = new UserRepository(mockContext.Object);

        //Act
        var result = await userRepository.Login("alice@test.com", "alice@test.comSD%^67");

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenPasswordNotMatch()
    {
        // Arrange
        var users = GetTestUsers();

        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        var userRepository = new UserRepository(mockContext.Object);

        //Act
        var result = await userRepository.Login("alice@test.com", "wrongPassword");

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenEmailNotMatch()
    {
        // Arrange
        var users = GetTestUsers();

        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        var userRepository = new UserRepository(mockContext.Object);

        //Act
        var result = await userRepository.Login("wrongEmail", "alice@test.comSD%^67");

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUser_ExistingUser_UpdatesUser()
    {
        // Arrange
        var users = GetTestUsers();

        var mockContext = new Mock<dbSHOPContext>();
        mockContext.Setup(x => x.Users).ReturnsDbSet(users);

        var userRepository = new UserRepository(mockContext.Object);

        var updatedUser = new User
        {
            UserId = 1,
            UserFirstName = "Alice",
            UserLastName = "Johnson2", // Changed last name
            UserEmail = "alice@test.com",
            UserPassword = "alice@test.comSD%^67",
        };

        // Act
        await userRepository.UpdateUser(updatedUser);

        // Assert
        mockContext.Verify(x => x.Users.Update(It.Is<User>(u => u.UserLastName == "Johnson2")), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}



