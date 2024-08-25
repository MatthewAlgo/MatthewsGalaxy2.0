using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.WorkerMicroservice.Controllers;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using MatthewsGalaxy.WorkerMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Subscriber = MatthewsGalaxy.Server.Models.Subscriber;
using EmailRequest = MatthewsGalaxy.WorkerMicroservice.Models.EmailRequest;

namespace MatthewsGalaxy.TestEmailMicroservice
{
    public class TestEmailController
    {
        private readonly Mock<IEmailRepository> _mockEmailRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly EmailController _controller;

        public TestEmailController()
        {
            _mockEmailRepository = new Mock<IEmailRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Setup configuration mock to return a base URL
            _mockConfiguration.Setup(config => config["Backend:BaseURL"])
                .Returns("http://localhost");

            _controller = new EmailController(
                _mockEmailRepository.Object,
                _mockEmailService.Object,
                _mockConfiguration.Object
            );
        }

        [Fact]
        public async Task SendVerificationEmailToUser_ReturnsOkResult()
        {
            // Arrange
            var destination = "test@example.com";
            var token = "test-token";

            _mockEmailService.Setup(service => service.SendVerificationEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendVerificationEmailToUser(destination, token);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SendVerificationEmailToUser_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var destination = "test@example.com";
            var token = "test-token";

            _mockEmailService.Setup(service => service.SendVerificationEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Email sending failed."));

            // Act
            var result = await _controller.SendVerificationEmailToUser(destination, token);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Email sending failed.", statusCodeResult.Value);
        }

        [Fact]
        public async Task SendEmail_ReturnsOkResult()
        {
            // Arrange
            var emailRequest = new EmailRequest()  {
                DestinationEmail = "test@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            _mockEmailRepository.Setup(repo => repo.SendEmailAsync(emailRequest.DestinationEmail, emailRequest.Subject, emailRequest.Body))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendEmail(emailRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SendEmail_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var emailRequest = new EmailRequest()
            {
                DestinationEmail = "test@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            _mockEmailRepository.Setup(repo => repo.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Email sending failed."));

            // Act
            var result = await _controller.SendEmail(emailRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Email sending failed.", statusCodeResult.Value);
        }

        [Fact]
        public async Task AddEmailAddressSubscriptions_ReturnsOkResult()
        {
            // Arrange
            var emailAddress = "test@example.com";
            var token = "test-token";

            _mockEmailRepository.Setup(repo => repo.AddEmailAddressSubscriptionsAsync(It.IsAny<Subscriber>()))
                .ReturnsAsync(true);

            _mockEmailRepository.Setup(repo => repo.GetVerificationEmailTokenizedIdSubscriptionsAsync(emailAddress))
                .ReturnsAsync(token);

            _mockEmailService.Setup(service => service.SendSubscriptionVerificationEmailAsync(emailAddress, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddEmailAddressSubscriptions(emailAddress);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email address added successfully! Please verify your email address.", okResult.Value);
        }

        [Fact]
        public async Task AddEmailAddressSubscriptions_ReturnsBadRequest_WhenAddingFails()
        {
            // Arrange
            var emailAddress = "test@example.com";

            _mockEmailRepository.Setup(repo => repo.AddEmailAddressSubscriptionsAsync(It.IsAny<Subscriber>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.AddEmailAddressSubscriptions(emailAddress);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add email address. Most likely your email already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task AddEmailAddressSubscriptions_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var emailAddress = "test@example.com";

            _mockEmailRepository.Setup(repo => repo.AddEmailAddressSubscriptionsAsync(It.IsAny<Subscriber>()))
                .Throws(new Exception("Adding subscription failed."));

            // Act
            var result = await _controller.AddEmailAddressSubscriptions(emailAddress);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Adding subscription failed.", statusCodeResult.Value);
        }

        [Fact]
        public async Task VerifyEmailAddressSubscriptions_ReturnsOkResult_WhenVerified()
        {
            // Arrange
            var token = "test-token";

            _mockEmailRepository.Setup(repo => repo.VerifyEmailAddressSubscriptionsAsync(token))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.VerifyEmailAddressSubscriptions(token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email address verified successfully!", okResult.Value);
        }

        [Fact]
        public async Task VerifyEmailAddressSubscriptions_ReturnsBadRequest_WhenVerificationFails()
        {
            // Arrange
            var token = "invalid-token";

            _mockEmailRepository.Setup(repo => repo.VerifyEmailAddressSubscriptionsAsync(token))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.VerifyEmailAddressSubscriptions(token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid token or verification failed.", badRequestResult.Value);
        }

        [Fact]
        public async Task VerifyEmailAddressSubscriptions_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var token = "test-token";

            _mockEmailRepository.Setup(repo => repo.VerifyEmailAddressSubscriptionsAsync(It.IsAny<string>()))
                .Throws(new Exception("Verification failed."));

            // Act
            var result = await _controller.VerifyEmailAddressSubscriptions(token);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Verification failed.", statusCodeResult.Value);
        }

        [Fact]
        public async Task RemoveEmailAddressSubscriptions_WithEmailRequest_ReturnsOkResult()
        {
            // Arrange
            var emailRequest = new VerificationEmailRequest
            {
                Email = "test@example.com",
                Token = "test-token"
            };

            _mockEmailRepository.Setup(repo => repo.RemoveEmailAddressSubscriptionsAsync(emailRequest))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveEmailAddressSubscriptions(emailRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email address removed successfully! You will no longer receive emails from me :(.", okResult.Value);
        }

        [Fact]
        public async Task RemoveEmailAddressSubscriptions_WithEmailRequest_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var emailRequest = new VerificationEmailRequest
            {
                Email = "test@example.com",
                Token = "test-token"
            };

            _mockEmailRepository.Setup(repo => repo.RemoveEmailAddressSubscriptionsAsync(It.IsAny<VerificationEmailRequest>()))
                .Throws(new Exception("Removal failed."));

            // Act
            var result = await _controller.RemoveEmailAddressSubscriptions(emailRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Removal failed.", statusCodeResult.Value);
        }

        [Fact]
        public async Task RemoveEmailAddressSubscriptions_WithEmailAndToken_ReturnsOkResult()
        {
            // Arrange
            var email = "test@example.com";
            var token = "test-token";

            _mockEmailRepository.Setup(repo => repo.RemoveEmailAddressSubscriptionsAsync(
                    new VerificationEmailRequest() { Email = email, Token = token }
                    ))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveEmailAddressSubscriptions(email, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email address removed successfully! You will no longer receive emails from me :(.", okResult.Value);
        }

        [Fact]
        public async Task RemoveEmailAddressSubscriptions_WithEmailAndToken_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var email = "test@example.com";
            var token = "test-token";

            _mockEmailRepository.Setup(repo => repo.RemoveEmailAddressSubscriptionsAsync(
                        It.IsAny<VerificationEmailRequest>()
                    ))
                .Throws(new Exception("Removal failed."));

            // Act
            var result = await _controller.RemoveEmailAddressSubscriptions(email, token);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Removal failed.", statusCodeResult.Value);
        }
    }
}