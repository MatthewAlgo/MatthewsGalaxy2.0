using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.Test
{
    public class TestCommentsController
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<CommentsController>> _mockLogger;
        private readonly Mock<IBlogPostRepository> _mockBlogPostRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly CommentsController _controller;

        public TestCommentsController()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<CommentsController>>();
            _mockBlogPostRepository = new Mock<IBlogPostRepository>();
            _mockUserService = new Mock<IUserService>();
            _controller = new CommentsController(
                _mockCommentRepository.Object,
                _mockUserRepository.Object,
                _mockLogger.Object,
                _mockBlogPostRepository.Object,
                _mockUserService.Object
            );
        }

        [Fact]
        public async Task GetComments_ReturnsOkResult_WithAListOfComments()
        {
            // Arrange
            var comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Content = "Comment 1" },
            new Comment { Id = Guid.NewGuid(), Content = "Comment 2" }
        };
            _mockCommentRepository.Setup(repo => repo.GetComments()).ReturnsAsync(comments.Select(
                comment => CommentDTO.FromComment(comment)).ToList());


            // Act
            var result = await _controller.GetComments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComments = Assert.IsType<List<CommentDTO>>(okResult.Value);
            Assert.Equal(2, returnComments.Count);
        }

        [Fact]
        public async Task GetCommentsByBlogPost_ReturnsOkResult_WithAListOfComments()
        {
            // Arrange
            var blogPostId = Guid.NewGuid();
            var comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Content = "Comment 1", BlogPostId = blogPostId },
            new Comment { Id = Guid.NewGuid(), Content = "Comment 2", BlogPostId = blogPostId }
        };
            var blogPost = new BlogPost { Id = blogPostId, Title = "Test Post", AuthorId = Guid.NewGuid().ToString() };
            var user = new User { Id = Guid.NewGuid().ToString(), Roles = new List<string> { "User" } };

            _mockBlogPostRepository.Setup(repo => repo.GetBlogPostsRaw()).ReturnsAsync(new List<BlogPost> { blogPost });
            _mockCommentRepository.Setup(repo => repo.GetCommentsByBlogPost(blogPostId)).ReturnsAsync(comments.Select(
                comment => CommentDTO.FromComment(comment)).ToList());
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);

            // Act
            var result = await _controller.GetCommentsByBlogPost(blogPostId.GetHashCode().ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComments = Assert.IsType<List<CommentDTO>>(okResult.Value);
            Assert.Equal(2, returnComments.Count);
        }

        [Fact]
        public async Task CreateComment_ReturnsCreatedResult_WhenCommentIsValid()
        {
            // Arrange
            var articleId = Guid.NewGuid().ToString();
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "testuser", Email = "testuser@example.com" };
            var commentDto = new CommentBodyDTO { Title = "Test Title", Content = "Test Content" };
            var newComment = new CommentDTO
            {
                Id = Guid.NewGuid(),
                Title = "Test Title",
                Content = "Test Content",
                AuthorId = user.Id,
                ArticleId = articleId,
                Date = DateTime.UtcNow
            };

            _mockBlogPostRepository.Setup(repo => repo.GetBlogPostsRaw()).ReturnsAsync(new List<BlogPost>
        {
            new BlogPost { Id = Guid.Parse(articleId), Title = "Test Post",  AuthorId = Guid.NewGuid().ToString()  }
        });
            _mockUserRepository.Setup(repo => repo.GetIdentityUser(user.UserName, user.Email)).ReturnsAsync(user);
            _mockCommentRepository.Setup(repo => repo.CreateComment(It.IsAny<CommentDTO>())).ReturnsAsync(newComment);

            // Act
            var result = await _controller.CreateComment(articleId.GetHashCode().ToString(), user.UserName, user.Email, commentDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnComment = Assert.IsType<CommentDTO>(createdResult.Value);
            Assert.Equal(newComment.Title, returnComment.Title);
        }

        [Fact]
        public async Task GetComment_ReturnsOkResult_WithComment()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, Content = "Test Comment" };
            _mockCommentRepository.Setup(repo => repo.GetComment(commentId)).ReturnsAsync(CommentDTO.FromComment(comment));

            // Act
            var result = await _controller.GetComment(commentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComment = Assert.IsType<CommentDTO>(okResult.Value);
            Assert.Equal(comment.Content, returnComment.Content);
        }

        [Fact]
        public async Task GetCommentsByUser_ReturnsOkResult_WithAListOfComments()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Content = "Comment 1", AuthorId = userId.ToString() },
            new Comment { Id = Guid.NewGuid(), Content = "Comment 2", AuthorId = userId.ToString() }
        };
            _mockCommentRepository.Setup(repo => repo.GetCommentsByUser(userId)).ReturnsAsync(comments.Select(comment => CommentDTO.FromComment(comment)).ToList());

            // Act
            var result = await _controller.GetCommentsByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComments = Assert.IsType<List<CommentDTO>>(okResult.Value);
            Assert.Equal(2, returnComments.Count);
        }

        [Fact]
        public async Task GetCommentsByUserName_ReturnsOkResult_WithAListOfComments()
        {
            // Arrange
            var userName = "testuser";
            var comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Content = "Comment 1", Author = new User { UserName = userName } },
            new Comment { Id = Guid.NewGuid(), Content = "Comment 2", Author = new User { UserName = userName } }
        };
            _mockCommentRepository.Setup(repo => repo.GetCommentsByUserName(userName)).ReturnsAsync(comments.Select(comment => CommentDTO.FromComment(comment)).ToList());

            // Act
            var result = await _controller.GetCommentsByUserName(userName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComments = Assert.IsType<List<CommentDTO>>(okResult.Value);
            Assert.Equal(2, returnComments.Count);
        }

        [Fact]
        public async Task GetCommentsByDate_ReturnsOkResult_WithAListOfComments()
        {
            // Arrange
            var date = "2024-07-31";
            var comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Content = "Comment 1", Date = DateTime.Parse(date) },
            new Comment { Id = Guid.NewGuid(), Content = "Comment 2", Date = DateTime.Parse(date) }
        };
            _mockCommentRepository.Setup(repo => repo.GetCommentsByDate(date)).ReturnsAsync(comments.Select(comment => CommentDTO.FromComment(comment)).ToList());

            // Act
            var result = await _controller.GetCommentsByDate(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComments = Assert.IsType<List<CommentDTO>>(okResult.Value);
            Assert.Equal(2, returnComments.Count);
        }

        [Fact]
        public async Task UpdateComment_ReturnsOkResult_WhenCommentIsUpdated()
        {
            // Arrange
            var comment = new Comment { Id = Guid.NewGuid(), Content = "Updated Comment" };
            _mockCommentRepository.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync(CommentDTO.FromComment(comment));

            // Act
            var result = await _controller.UpdateComment(comment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComment = Assert.IsType<CommentDTO>(okResult.Value);
            Assert.Equal(comment.Content, returnComment.Content);
        }

        [Fact]
        public async Task DeleteComment_ReturnsOkResult_WhenCommentIsDeleted()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, Content = "Comment to delete" };
            var user = new User { Id = Guid.NewGuid().ToString(), Roles = new List<string> { "Admin" } };

            _mockCommentRepository.Setup(repo => repo.GetComment(commentId)).ReturnsAsync(CommentDTO.FromComment(comment));
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(user);
            _mockCommentRepository.Setup(repo => repo.DeleteComment(commentId)).ReturnsAsync(CommentDTO.FromComment(comment));

            // Act
            var result = await _controller.DeleteComment(commentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnComment = Assert.IsType<CommentDTO>(okResult.Value);
            Assert.Equal(comment.Content, returnComment.Content);
        }

        [Fact]
        public async Task DeleteComment_ReturnsUnauthorizedResult_WhenCommentIdDeletedByOtherUser()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new Comment
            {
                Id = commentId,
                Content = "Comment to delete",
                AuthorId = Guid.NewGuid().ToString(),
            };

            var guestUser = new User { Id = Guid.NewGuid().ToString(), Roles = new List<string> { "Guest" } };

            _mockCommentRepository.Setup(repo => repo.GetComment(It.IsAny<Guid>())).ReturnsAsync(CommentDTO.FromComment(comment));
            _mockUserService.Setup(service => service.GetCurrentUserAsync()).ReturnsAsync(guestUser);

            // Act
            var result = await _controller.DeleteComment(commentId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        

    }
}
