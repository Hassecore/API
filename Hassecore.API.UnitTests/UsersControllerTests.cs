using Hassecore.API.Business.DTOs.UserPairing;
using Hassecore.API.Controllers;
using Hassecore.API.Data.Context.CurrentUserContext;
using Hassecore.API.Data.Entities.UserPairing;
using Hassecore.API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Hassecore.API.UnitTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IBaseRepository> _mockRepository;
        private readonly Mock<CurrentUserContext> _mockCurrentUserContext;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockRepository = new Mock<IBaseRepository>();
            _mockCurrentUserContext = new Mock<CurrentUserContext>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_mockRepository.Object, _mockCurrentUserContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "ext-001",
                    Username = "user1",
                    Email = "user1@example.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastOnline = DateOnly.FromDateTime(DateTime.UtcNow)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "ext-002",
                    Username = "user2",
                    Email = "user2@example.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastOnline = DateOnly.FromDateTime(DateTime.UtcNow)
                }
            }.AsQueryable();

            _mockRepository.Setup(repo => repo.GetQueryable<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(users);

            // Act
            var result = await _controller.GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<List<User>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<User>().AsQueryable();

            _mockRepository.Setup(repo => repo.GetQueryable<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(users);

            // Act
            var result = await _controller.GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<List<User>>(okResult.Value);
            Assert.Empty(returnedUsers);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User 
            { 
                Id = userId,
                ExternalId = "ext-001",
                Username = "testuser",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastOnline = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _mockRepository.Setup(repo => repo.GetAsync<User>(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetAsync<User>(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetUserAsync(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //[Fact]
        //public void GetUserInfoAsync_ReturnsOkResult_WithUserPairDto()
        //{
        //    // Arrange
        //    var currentUserId = Guid.NewGuid();
        //    var pairedUserId = Guid.NewGuid();

        //    _mockCurrentUserContext.Setup(ctx => ctx.UserId).Returns(currentUserId);
        //    _mockCurrentUserContext.Setup(ctx => ctx.PairedUserId).Returns(pairedUserId);

        //    // Act
        //    var result = _controller.GetUserInfoAsync(Guid.NewGuid());

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var userInfo = Assert.IsType<UserPairDto>(okResult.Value);
        //    Assert.Equal(currentUserId, userInfo.CurrentUserId);
        //    Assert.Equal(pairedUserId, userInfo.PairedUserid);
        //}

        //[Fact]
        //public async Task GetUsersAsync_LogsInformation_OnSuccess()
        //{
        //    // Arrange
        //    var users = new List<User>().AsQueryable();
        //    _mockRepository.Setup(repo => repo.GetQueryable<User>(It.IsAny<Expression<Func<User, bool>>>()))
        //        .Returns(users);

        //    // Act
        //    await _controller.GetUsersAsync();

        //    // Assert
        //    _mockLogger.Verify(
        //        x => x.Log(
        //            LogLevel.Information,
        //            It.IsAny<EventId>(),
        //            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Users retrieved successfully")),
        //            It.IsAny<Exception>(),
        //            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        //        Times.Once);
        //}

        [Fact]
        public async Task GetUserAsync_LogsWarning_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetAsync<User>(userId))
                .ReturnsAsync((User?)null);

            // Act
            await _controller.GetUserAsync(userId);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}