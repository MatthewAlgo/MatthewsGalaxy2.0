using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using MatthewsGalaxy.WorkerMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.WorkerMicroservice.Controllers;
using MatthewsGalaxy.WorkerMicroservice.Models;
using MatthewsGalaxy.WorkerMicroservice.Repository;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.TestEmailMicroservice
{
    public class TestSubscribersController
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IEmailRepository> _mockEmailRepository;
        private readonly SubscribersController _controller;
        // Mock httpclient

        public TestSubscribersController()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockApiService = new Mock<IApiService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockEmailRepository = new Mock<IEmailRepository>();

            _controller = new SubscribersController(
                _mockUnitOfWork.Object,
                _mockApiService.Object,
                _mockEmailService.Object,
                _mockEmailRepository.Object
            );
        }

        [Fact]
        public async Task GetSubscribers_ReturnsOkResult_WithListOfSubscribers()
        {
            // Arrange
            var subscribers = new List<Server.Models.Subscriber>
            {
                new Server.Models.Subscriber { Id = Guid.NewGuid(), Email = "test1@example.com" },
                new Server.Models.Subscriber { Id = Guid.NewGuid(), Email = "test2@example.com" }
            }.AsQueryable();

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(subscribers); 

            // Act
            var result = await _controller.GetSubscribers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Server.Models.Subscriber>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetSubscriberById_ReturnsOkResult_WithSubscriber()
        {
            // Arrange
            var subscriber = new Server.Models.Subscriber { Id = Guid.NewGuid(), Email = "test@example.com" };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(subscriber.Id))
                .ReturnsAsync(subscriber);

            // Act
            var result = await _controller.GetSubscriberById(subscriber.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Server.Models.Subscriber>(okResult.Value);
            Assert.Equal(subscriber.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetSubscriberById_ReturnsNotFoundResult_WhenSubscriberDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(new List<Server.Models.Subscriber>().AsQueryable());

            // Act
            var result = await _controller.GetSubscriberById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetSubscriberByEmail_ReturnsOkResult_WithSubscriber()
        {
            // Arrange
            var subscriber = new Server.Models.Subscriber { Id = Guid.NewGuid(), Email = "test@example.com" };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(new List<Server.Models.Subscriber> { subscriber }.AsQueryable());

            // Act
            var result = await _controller.GetSubscriberByEmail(subscriber.Email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Server.Models.Subscriber>(okResult.Value);
            Assert.Equal(subscriber.Email, returnValue.Email);
        }

        [Fact]
        public async Task GetSubscriberByEmail_ReturnsNotFoundResult_WhenSubscriberDoesNotExist()
        {
            var subscribers = new List<Server.Models.Subscriber>().AsQueryable();
            // Arrange
            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(subscribers);

            // Act
            var result = await _controller.GetSubscriberByEmail("nonexistent@example.com");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddSubscriber_ReturnsOkResult_WhenSubscriberIsAdded()
        {
            // Arrange
            var newSubscriber = new Server.Models.Subscriber { Email = "new@example.com" };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(new List<Server.Models.Subscriber>().AsQueryable());

            _mockUnitOfWork.Setup(uow => uow.Subscribers.AddAsync(It.IsAny<Server.Models.Subscriber>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.SaveAsync())
                .ReturnsAsync(0);

            // Act
            var result = await _controller.AddSubscriber(newSubscriber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Server.Models.Subscriber>(okResult.Value);
            Assert.Equal(newSubscriber.Email, returnValue.Email);
        }

        [Fact]
        public async Task AddSubscriber_ReturnsBadRequest_WhenSubscriberIsNull()
        {
            // Act
            var result = await _controller.AddSubscriber(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddSubscriber_ReturnsBadRequest_WhenEmailAlreadySubscribed()
        {
            // Arrange
            var existingSubscriber = new Server.Models.Subscriber { Email = "existing@example.com" };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(new List<Server.Models.Subscriber> { existingSubscriber }.AsQueryable());

            // Act
            var result = await _controller.AddSubscriber(existingSubscriber);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The email is already subscribed.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteSubscriber_ReturnsNoContentResult_WhenSubscriberIsDeleted()
        {
            // Arrange
            var subscriberId = Guid.NewGuid();
            var subscriber = new Server.Models.Subscriber { Id = subscriberId, Email = "test@example.com" };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(subscriberId))
                .ReturnsAsync(subscriber);

            _mockUnitOfWork.Setup(uow => uow.Subscribers.Delete(It.IsAny<Server.Models.Subscriber>()));

            _mockUnitOfWork.Setup(uow => uow.SaveAsync())
                .ReturnsAsync(0);

            // Act
            var result = await _controller.DeleteSubscriber(subscriberId.ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSubscriber_ReturnsNotFoundResult_WhenSubscriberDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Server.Models.Subscriber)null);

            // Act
            var result = await _controller.DeleteSubscriber(Guid.NewGuid().ToString());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SendEmailToSubscribers_ReturnsOkResult()
        {
            // Arrange
            var subscribers = new List<Server.Models.Subscriber>
        {
            new Server.Models.Subscriber { Email = "test1@example.com" },
            new Server.Models.Subscriber { Email = "test2@example.com" }
        };

            var emailRequest = new EmailRequest()
            {
                Subject = "Test Subject",
                Body = "Test Body"
            };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetAllAsync())
                .ReturnsAsync(subscribers.AsQueryable());

            _mockEmailRepository.Setup(repo => repo.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendEmailToSubscribers(emailRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SendEmailToSubscriber_ReturnsOkResult_WhenEmailIsSent()
        {
            // Arrange
            var subscriberId = Guid.NewGuid();
            var subscriber = new Server.Models.Subscriber() { Id = subscriberId, Email = "test@example.com" };

            var emailRequest = new EmailRequest()
            {
                Subject = "Test Subject",
                Body = "Test Body"
            };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(subscriberId))
                .ReturnsAsync(subscriber);

            _mockEmailRepository.Setup(repo => repo.SendEmailAsync(subscriber.Email, emailRequest.Subject, emailRequest.Body))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendEmailToSubscriber(subscriberId.ToString(), emailRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SendEmailToSubscriber_ReturnsNotFoundResult_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var emailRequest = new EmailRequest
            {
                Subject = "Test Subject",
                Body = "Test Body"
            };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Server.Models.Subscriber)null);

            // Act
            var result = await _controller.SendEmailToSubscriber(Guid.NewGuid().ToString(), emailRequest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateSubscriber_ReturnsOkResult_WhenSubscriberIsUpdated()
        {
            // Arrange
            var subscriber = new Server.Models.Subscriber
            {
                Id = Guid.NewGuid(),
                Email = "old@example.com",
                Verified = false
            };

            var updatedSubscriber = new Server.Models.Subscriber
            {
                Id = subscriber.Id,
                Email = "new@example.com",
                Verified = true
            };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(subscriber.Id))
                .ReturnsAsync(subscriber);

            _mockUnitOfWork.Setup(uow => uow.Subscribers.Update(It.IsAny<Server.Models.Subscriber>()));

            _mockUnitOfWork.Setup(uow => uow.SaveAsync())
                .ReturnsAsync(0);

            // Act
            var result = await _controller.UpdateSubscriber(updatedSubscriber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Server.Models.Subscriber>(okResult.Value);
            Assert.Equal(updatedSubscriber.Email, returnValue.Email);
            Assert.Equal(updatedSubscriber.Verified, returnValue.Verified);
        }

        [Fact]
        public async Task UpdateSubscriber_ReturnsNotFoundResult_WhenSubscriberDoesNotExist()
        {
            // Arrange
            var subscriber = new Server.Models.Subscriber
            {
                Id = Guid.NewGuid(),
                Email = "nonexistent@example.com",
                Verified = true
            };

            _mockUnitOfWork.Setup(uow => uow.Subscribers.GetByIdAsync(subscriber.Id))
                .ReturnsAsync((Server.Models.Subscriber)null);

            // Act
            var result = await _controller.UpdateSubscriber(subscriber);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
