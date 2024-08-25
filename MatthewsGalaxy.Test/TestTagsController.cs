using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.DTOs;

namespace MatthewsGalaxy.Test
{
    public class TestTagsController
    {
        private readonly Mock<ITagRepository> _mockRepo;
        private readonly TagsController _controller;

        public TestTagsController()
        {
            _mockRepo = new Mock<ITagRepository>();
            _controller = new TagsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetTags_ReturnsOkResult_WithListOfTags()
        {
            // Arrange
            var tags = new List<Tag> { new Tag { Id = Guid.NewGuid(), TagName = "TestTag" } };
            _mockRepo.Setup(repo => repo.GetTags()).ReturnsAsync(tags.Select(t => TagDTO.FromPostTag(t)).ToList());

            // Act
            var result = await _controller.GetTags();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTags = Assert.IsType<List<TagDTO>>(okResult.Value);
            Assert.Single(returnTags);
        }

        [Fact]
        public async Task GetTag_ById_ReturnsOkResult_WithTag()
        {
            // Arrange
            var tagId = Guid.NewGuid();
            var tag = new Tag { Id = tagId, TagName = "TestTag" };
            _mockRepo.Setup(repo => repo.GetTag(tagId)).ReturnsAsync(TagDTO.FromPostTag(tag));

            // Act
            var result = await _controller.GetTag(tagId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTag = Assert.IsType<TagDTO>(okResult.Value);
            Assert.Equal(tag.TagName, returnTag.TagName);
        }

        [Fact]
        public async Task GetTagsByBlogPost_ReturnsOkResult_WithListOfTags()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var tags = new List<Tag> { new Tag { Id = Guid.NewGuid(), TagName = "TestTag" } };
            _mockRepo.Setup(repo => repo.GetTagsByBlogPost(postId)).ReturnsAsync(tags.Select(t => TagDTO.FromPostTag(t)).ToList());

            // Act
            var result = await _controller.GetTagsByBlogPost(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTags = Assert.IsType<List<TagDTO>>(okResult.Value);
            Assert.Single(returnTags);
        }

        [Fact]
        public async Task GetTag_ByTagName_ReturnsOkResult_WithTag()
        {
            // Arrange
            var tagName = "TestTag";
            var tags = new List<Tag> { new Tag { Id = Guid.NewGuid(), TagName = tagName } };
            _mockRepo.Setup(repo => repo.GetTagsByTagName(tagName)).ReturnsAsync(tags.Select(t => TagDTO.FromPostTag(t)).ToList());

            // Act
            var result = await _controller.GetTag(tagName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTags = Assert.IsType<List<TagDTO>>(okResult.Value);
            Assert.Single(returnTags);
        }

        [Fact]
        public async Task CreateTag_ReturnsCreatedResult_WithTag()
        {
            // Arrange
            var tag = new Tag { Id = Guid.NewGuid(), TagName = "NewTag" };
            _mockRepo.Setup(repo => repo.CreateTag(tag)).Returns(
                Task.FromResult(TagDTO.FromPostTag(tag))
            );

            // Act
            var result = await _controller.CreateTag(tag);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnTag = Assert.IsType<Tag>(createdResult.Value);
            Assert.Equal(tag.TagName, returnTag.TagName);
        }

        [Fact]
        public async Task UpdateTag_ReturnsOkResult_WithUpdatedTag()
        {
            // Arrange
            var tag = new Tag { Id = Guid.NewGuid(), TagName = "UpdatedTag" };
            _mockRepo.Setup(repo => repo.UpdateTag(tag)).ReturnsAsync(TagDTO.FromPostTag(tag));

            // Act
            var result = await _controller.UpdateTag(tag);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTag = Assert.IsType<TagDTO>(okResult.Value);
            Assert.Equal(tag.TagName, returnTag.TagName);
        }

        [Fact]
        public async Task DeleteTag_ReturnsOkResult_WithDeletedTag()
        {
            // Arrange
            var tagId = Guid.NewGuid();
            var tag = new Tag { Id = tagId, TagName = "DeletedTag" };
            _mockRepo.Setup(repo => repo.DeleteTag(tagId)).ReturnsAsync(TagDTO.FromPostTag(tag));

            // Act
            var result = await _controller.DeleteTag(tagId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTag = Assert.IsType<TagDTO>(okResult.Value);
            Assert.Equal(tag.TagName, returnTag.TagName);
        }
    }
}
