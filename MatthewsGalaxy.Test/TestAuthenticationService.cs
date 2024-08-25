using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.DTOs.Auth;
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MatthewsGalaxy.Test
{
    public class TestAuthenticationService
    {
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly UserController _userController;

        public TestAuthenticationService()
        {
            _mockAuthService = new Mock<IAuthenticationService>();
            _userController = new UserController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithValidRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest { Username = "testuser", Password = "password" };
            var tokenResponse = new TokenResponse { Token = "validToken", UserName = "testuser", UserEmail = "testuser@example.com", UserRole = "Guest" };
            _mockAuthService.Setup(s => s.Login(It.IsAny<LoginRequest>())).ReturnsAsync(tokenResponse);

            // Act
            var result = await _userController.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<TokenResponse>(okResult.Value);
            Assert.Equal("validToken", response.Token);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_OnException()
        {
            // Arrange
            var loginRequest = new LoginRequest { Username = "invaliduser", Password = "password" };
            _mockAuthService.Setup(s => s.Login(It.IsAny<LoginRequest>())).ThrowsAsync(new ArgumentException("Invalid credentials"));

            // Act
            var result = await _userController.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WithValidRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequest { Email = "testuser@example.com", Username = "testuser", Password = "password" };
            var tokenResponse = new TokenResponse { Token = "validToken", UserName = "testuser", UserEmail = "testuser@example.com", UserRole = "Guest" };
            _mockAuthService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ReturnsAsync(tokenResponse);

            // Act
            var result = await _userController.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<TokenResponse>(okResult.Value);
            Assert.Equal("validToken", response.Token);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_OnException()
        {
            // Arrange
            var registerRequest = new RegisterRequest { Email = "existinguser@example.com", Username = "existinguser", Password = "password" };
            _mockAuthService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ThrowsAsync(new ArgumentException("User already exists"));

            // Act
            var result = await _userController.Register(registerRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsInternalServerError_OnUnhandledException()
        {
            // Arrange
            var registerRequest = new RegisterRequest { Email = "testuser@example.com", Username = "testuser", Password = "password" };
            _mockAuthService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ThrowsAsync(new Exception("Unhandled exception"));

            // Act
            var result = await _userController.Register(registerRequest);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("Unhandled exception", internalServerErrorResult.Value);
        }
    }
}
