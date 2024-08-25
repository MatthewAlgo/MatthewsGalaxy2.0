using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.Test
{
    public class TestUsersController
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public TestUsersController()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserRepo.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetCurrentUser_ReturnsOkResult_WithUser()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser" };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.GetCurrentUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.UserName, returnUser.UserName);
        }

        [Fact]
        public async Task GetUsers_WhenNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser", Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetUsers_WhenAdmin_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var admin = new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Roles = new List<string> { "Admin" } };
            var users = new List<User> { new User { Id = Guid.NewGuid().ToString(), UserName = "testuser" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(admin);
            _mockUserRepo.Setup(repo => repo.GetUsers()).ReturnsAsync(
                users.Select(u => UserDTO.FromUser(u)).ToList());

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsType<List<UserDTO>>(okResult.Value);
            Assert.Single(returnUsers);
        }

        [Fact]
        public async Task GetUser_ById_WhenNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser", Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(Guid.NewGuid());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetUser_ById_WhenAdmin_ReturnsOkResult_WithUser()
        {
            // Arrange
            var admin = new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Roles = new List<string> { "Admin" } };
            var userId = Guid.NewGuid();
            var user = new User { Id = userId.ToString(), UserName = "testuser" };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(admin);
            _mockUserRepo.Setup(repo => repo.GetUser(userId)).ReturnsAsync(
                UserDTO.FromUser(user));

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(user.UserName , returnUser.UserName);

        }

        [Fact]
        public async Task CreateUser_WhenNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser", Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.CreateUser(new User { UserName = "newuser" });

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateUser_WhenAdmin_ReturnsCreatedResult_WithUser()
        {
            // Arrange
            var admin = new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Roles = new List<string> { "Admin" } };
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "newuser" };
            var userDto = UserDTO.FromUser(user);
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(admin);
            _mockUserRepo.Setup(repo => repo.CreateUser(It.IsAny<UserDTO>())).ReturnsAsync(userDto);

            // Act
            var result = await _controller.CreateUser(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnUser = Assert.IsType<UserDTO>(createdResult.Value);
            Assert.Equal(user.UserName, returnUser.UserName);
        }

        [Fact]
        public async Task UpdateUser_WhenNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser", Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateUser(new User { UserName = "updateduser" });

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateUser_WhenAdmin_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            var admin = new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Roles = new List<string> { "Admin" } };
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "updateduser" };
            var userDto = UserDTO.FromUser(user);
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(admin);
            _mockUserRepo.Setup(repo => repo.UpdateUser(It.IsAny<UserDTO>())).ReturnsAsync(userDto);

            // Act
            var result = await _controller.UpdateUser(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(user.UserName, returnUser.UserName);
        }

        [Fact]
        public async Task DeleteUser_WhenNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser", Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.DeleteUser(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WhenAdmin_ReturnsOkResult_WithDeletedUser()
        {
            // Arrange
            var admin = new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Roles = new List<string> { "Admin" } };
            var userId = Guid.NewGuid();
            var user = new User { Id = userId.ToString(), UserName = "deleteduser" };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(admin);
            _mockUserRepo.Setup(repo => repo.GetUser(userId)).ReturnsAsync(UserDTO.FromUser(user));
            _mockUserRepo.Setup(repo => repo.DeleteUser(userId.ToString())).ReturnsAsync(UserDTO.FromUser(user));

            // Act
            var result = await _controller.DeleteUser(userId.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(user.UserName, returnUser.UserName);
        }
    }
}
