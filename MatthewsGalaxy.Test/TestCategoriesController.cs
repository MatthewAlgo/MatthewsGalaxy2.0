using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.Test
{
    public class TestCategoriesController
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly CategoriesController _controller;

        public TestCategoriesController()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUserService = new Mock<IUserService>();
            _controller = new CategoriesController(_mockCategoryRepository.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "Category1" } };
            _mockCategoryRepository.Setup(repo => repo.GetCategories())
                .ReturnsAsync(categories.Select(CategoryDTO.FromCategory).ToList());

            // Act
            var result = await _controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategories = Assert.IsType<List<CategoryDTO>>(okResult.Value);
            Assert.Single(returnCategories);
        }

        [Fact]
        public async Task GetCategory_ById_ReturnsOkResult_WithCategory()
        {
            // Arrange
            var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
            _mockCategoryRepository.Setup(repo => repo.GetCategory(It.IsAny<Guid>()))
                .ReturnsAsync(CategoryDTO.FromCategory(category));

            // Act
            var result = await _controller.GetCategory(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(category.Name, returnCategory.Name);
        }

        [Fact]
        public async Task GetCategory_ByUrlHandle_ReturnsOkResult_WithCategory()
        {
            // Arrange
            var category = new Category { Id = Guid.NewGuid(), Name = "Category1" };
            _mockCategoryRepository.Setup(repo => repo.GetCategory(It.IsAny<string>()))
                .ReturnsAsync(CategoryDTO.FromCategory(category));

            // Act
            var result = await _controller.GetCategory("url-handle");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(category.Name, returnCategory.Name);
        }

        [Fact]
        public async Task CreateCategory_IfNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "NewCategory" };
            var user = new User { Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.CreateCategory(categoryDTO);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateCategory_IfAdmin_ReturnsOkResult_WithCategory()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "NewCategory" };
            var user = new User { Roles = new List<string> { "Admin" } };
            var newCategory = new Category { Id = Guid.NewGuid(), Name = categoryDTO.Name };

            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);
            _mockCategoryRepository.Setup(repo => repo.CreateCategory(It.IsAny<CategoryDTO>()))
                .ReturnsAsync(CategoryDTO.FromCategory(newCategory));

            // Act
            var result = await _controller.CreateCategory(categoryDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(newCategory.Name, returnCategory.Name);
        }

        [Fact]
        public async Task UpdateCategory_IfNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "UpdatedCategory" };
            var user = new User { Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateCategory(categoryDTO);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_IfAdmin_ReturnsOkResult_WithCategory()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "UpdatedCategory" };
            var user = new User { Roles = new List<string> { "Admin" } };
            var updatedCategory = new Category { Id = Guid.NewGuid(), Name = categoryDTO.Name };

            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);
            _mockCategoryRepository.Setup(repo => repo.UpdateCategory(It.IsAny<Category>()))
                .ReturnsAsync(CategoryDTO.FromCategory(updatedCategory));

            // Act
            var result = await _controller.UpdateCategory(categoryDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(updatedCategory.Name, returnCategory.Name);
        }

        [Fact]
        public async Task DeleteCategory_IfNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Roles = new List<string> { "User" } };
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.DeleteCategory(Guid.NewGuid());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_IfAdmin_ReturnsOkResult_WithCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var user = new User { Roles = new List<string> { "Admin" } };
            var deletedCategory = new Category { Id = categoryId, Name = "DeletedCategory" };

            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);
            _mockCategoryRepository.Setup(repo => repo.DeleteCategory(It.IsAny<Guid>()))
                .ReturnsAsync(CategoryDTO.FromCategory(deletedCategory));

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal(deletedCategory.Name, returnCategory.Name);
        }
    }
}
