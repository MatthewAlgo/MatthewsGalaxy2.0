using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.Test;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Test
{
    public class TestProjectController
    {
        private readonly Mock<IProjectRepository> _mockRepo;
        private readonly ProjectController _controller;

        public TestProjectController()
        {
            _mockRepo = new Mock<IProjectRepository>();
            _controller = new ProjectController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetProjects_ReturnsOkResult_WithListOfProjects()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = "1", Name = "Project1" } };
            _mockRepo.Setup(repo => repo.GetProjectsAsync()).ReturnsAsync(projects);

            // Act
            var result = await _controller.GetProjects();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProjects = Assert.IsType<List<Project>>(okResult.Value);
            Assert.Single(returnProjects);
        }

        [Fact]
        public async Task GetProjectById_ReturnsOkResult_WithProject()
        {
            // Arrange
            var project = new Project { Id = "1", Name = "Project1" };
            _mockRepo.Setup(repo => repo.GetProjectById("1")).ReturnsAsync(project);

            // Act
            var result = await _controller.GetProjectById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProject = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("1", returnProject.Id);
        }

        [Fact]
        public async Task GetProjectById_ReturnsNotFoundResult_WhenProjectNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetProjectById("1")).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetProjectById("1");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Project with ID '1' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateProject_ReturnsCreatedAtActionResult_WithCreatedProject()
        {
            // Arrange
            var projectDto = new ProjectDTO { Name = "Project1" };
            var project = new Project { Id = "1", Name = "Project1" };
            _mockRepo.Setup(repo => repo.CreateProject(projectDto)).ReturnsAsync(project);

            // Act
            var result = await _controller.CreateProject(projectDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnProject = Assert.IsType<Project>(createdAtActionResult.Value);
            Assert.Equal("1", returnProject.Id);
        }

        [Fact]
        public async Task CreateProject_ReturnsBadRequest_WhenProjectIsNull()
        {
            // Act
            var result = await _controller.CreateProject(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Project cannot be null.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProject_ReturnsOkResult_WithUpdatedProject()
        {
            // Arrange
            var project = new Project { Id = "1", Name = "UpdatedProject" };
            _mockRepo.Setup(repo => repo.UpdateProject(project)).ReturnsAsync(project);

            // Act
            var result = await _controller.UpdateProject("1", project);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProject = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("UpdatedProject", returnProject.Name);
        }

        [Fact]
        public async Task UpdateProject_ReturnsBadRequest_WhenProjectDataIsInvalid()
        {
            // Arrange
            var project = new Project { Id = "2", Name = "UpdatedProject" };

            // Act
            var result = await _controller.UpdateProject("1", project);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Project data is invalid.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteProject_ReturnsOkResult_WithDeletedProject()
        {
            // Arrange
            var project = new Project { Id = "1", Name = "Project1" };
            _mockRepo.Setup(repo => repo.DeleteProject("1")).ReturnsAsync(project);

            // Act
            var result = await _controller.DeleteProject("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProject = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("1", returnProject.Id);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNotFoundResult_WhenProjectNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteProject("1")).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteProject("1");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Project with ID '1' not found.", notFoundResult.Value);
        }
    }
}
