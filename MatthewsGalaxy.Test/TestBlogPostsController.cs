using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using MatthewsGalaxy.Server.Controllers;
using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.Server.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MatthewsGalaxy.Test
{

    public class TestBlogPostsController
    {
        private Mock<IBlogPostRepository> _blogPostRepository;
        private Mock<IBlogPostService> _blogPostService;
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserService> _userService;
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<ITagRepository> _tagRepository;
        private ApplicationDbContext _dbContext;

        private Fixture _fixture;

        private BlogPostsController _blogPostsController;

        public TestBlogPostsController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("MatthewsGalaxyTestDb");
            _dbContext = new ApplicationDbContext(optionsBuilder.Options);

            _blogPostRepository = new Mock<IBlogPostRepository>();
            _blogPostService = new Mock<IBlogPostService>();
            _userRepository = new Mock<IUserRepository>();
            _userService = new Mock<IUserService>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _tagRepository = new Mock<ITagRepository>();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task TestGetBlogPosts()
        {
            // Arrange
            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPosts()).ReturnsAsync(blogPosts);
            _blogPostsController = new BlogPostsController
            (_blogPostRepository.Object, _blogPostService.Object, _userRepository.Object, _userService.Object,
                _dbContext, _categoryRepository.Object, _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPosts();
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public async Task TestGetBlogPostsBySearchTerm()
        {
            // Arrange
            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPostsBySearchTerm(It.IsAny<string>())).ReturnsAsync(blogPosts);
            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPostsBySearchTerm("searchTerm");
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public async Task TestGetBlogPostsByTagId()
        {
            // Arrange
            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPostsByTagId(It.IsAny<string>())).ReturnsAsync(blogPosts);
            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPostsByTagId("tagId");
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public async Task TestGetBlogPostsByAuthor()
        {
            // Arrange
            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPostsByAuthor(It.IsAny<string>())).ReturnsAsync(blogPosts);
            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPostsByAuthor("author");
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
        }


        [Fact]
        public async Task TestGetBlogPostsByDate()
        {
            // Arrange
            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPostsByDate(It.IsAny<string>())).ReturnsAsync(blogPosts);
            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPostsByDate("date");
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public async Task TestGetBlogPostsByViews()
        {
            // Arrange
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };
            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);

            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPostsByViews(It.IsAny<int>())).ReturnsAsync(blogPosts);
            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPostsByViews(1);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task TestGetBlogPostsByLikes()
        {
            // Arrange
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };
            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            var blogPosts = _fixture.CreateMany<BlogPostDTO>().ToList();
            _blogPostRepository.Setup(bp => bp.GetBlogPostsByLikes(It.IsAny<int>())).ReturnsAsync(blogPosts);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.GetBlogPostsByLikes(1);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task TestLikeBlogPostByOtherUser()
        {
            // Arrange
            var blogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Test Title",
                ShortDescription = "Test Short Description",
                Description = "Test Description",
                FeaturedImageURL = "http://example.com/image.jpg",
                UrlHandle = "test-url",
                PublishedDate = DateTime.UtcNow,
                Views = 0,
                Likes = 0,
                AuthorId = "id",
                IsVisible = true,
                IsFeatured = false
            };

            // Create a dummy user with admin role
            var user = new SiteAdmin()
            {
                Id = "id",
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };
            blogPost.AuthorId = user.Id;

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostService.Setup(bp => bp.LikeBlogPost(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(BlogPostDTO.FromBlogPost(blogPost));

            // GetBlogPostsRaw() only returns the blogPost
            _blogPostRepository.Setup(bp => bp.GetBlogPostsRaw()).ReturnsAsync(new List<BlogPost> { blogPost });

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.LikeBlogPost(blogPost.Id.GetHashCode().ToString(), "user", "email");
            
            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        // Test unlike blog post by other user
        [Fact]
        public async Task TestUnlikeBlogPostByOtherUser()
        {
            // Arrange
            var blogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "Test Title",
                ShortDescription = "Test Short Description",
                Description = "Test Description",
                FeaturedImageURL = "http://example.com/image.jpg",
                UrlHandle = "test-url",
                PublishedDate = DateTime.UtcNow,
                Views = 0,
                Likes = 0,
                AuthorId = "id",
                IsVisible = true,
                IsFeatured = false
            };

            // Create a dummy user with admin role
            var user = new SiteAdmin()
            {
                Id = "id",
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };
            blogPost.AuthorId = user.Id;

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostService.Setup(bp => bp.UnlikeBlogPost(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(BlogPostDTO.FromBlogPost(blogPost));

            // GetBlogPostsRaw() only returns the blogPost
            _blogPostRepository.Setup(bp => bp.GetBlogPostsRaw()).ReturnsAsync(new List<BlogPost> { blogPost });

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.UnlikeBlogPost(blogPost.Id.GetHashCode().ToString(), "user", "email");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }


        [Fact]
        public async Task TestUnlikeBlogPostByAdmin()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with admin role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostService.Setup(bp => bp.UnlikeBlogPost(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.UnlikeBlogPost("id", "username", "useremail");
            var obj = result as ObjectResult;

            // Assert ok
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public async Task TestLikeBlogPostByGuestUser()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with guest role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Guest" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostService.Setup(bp => bp.LikeBlogPost(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.LikeBlogPost("id", "username", "useremail");
            var obj = result as ObjectResult;

            // Assert 
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public async Task TestUnlikeBlogPostByGuestUser()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with guest role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Guest" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostService.Setup(bp => bp.UnlikeBlogPost(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.UnlikeBlogPost("id", "username", "useremail");
            var obj = result as ObjectResult;

            // Assert 
            Assert.Equal(200, obj.StatusCode);
        }

        // Test create blog post
        [Fact]
        public async Task TestCreateBlogPost()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with admin role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostRepository.Setup(bp => bp.CreateBlogPost(It.IsAny<BlogPostDTO>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.CreateBlogPost(BlogPostBodyDTO.FromBlogPostDTO(blogPost));
            var obj = result as ObjectResult;

            // Assert 
            Assert.Equal(201, obj.StatusCode);
        }

        // Test create blog post by guest user
        [Fact]
        public async Task TestCreateBlogPostByGuestUser()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with guest role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Guest" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostRepository.Setup(bp => bp.CreateBlogPost(It.IsAny<BlogPostDTO>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.CreateBlogPost(BlogPostBodyDTO.FromBlogPostDTO(blogPost));

            // Assert 
            Assert.IsType<UnauthorizedResult>(result);
        }

        // Test update blog post
        [Fact]
        public async Task TestUpdateBlogPost()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with admin role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Admin" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostRepository.Setup(bp => bp.UpdateBlogPost(It.IsAny<BlogPostDTO>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.UpdateBlogPost(BlogPostBodyDTO.FromBlogPostDTO(blogPost));
            var obj = result as ObjectResult;

            // Assert 
            Assert.Equal(200, obj.StatusCode);
        }

        // Test update blog post by guest user
        [Fact]
        public async Task TestUpdateBlogPostByGuestUser()
        {
            // Arrange
            var blogPost = _fixture.Create<BlogPostDTO>();
            // Create a dummy user with guest role
            var user = new User
            {
                UserName = "username",
                Email = "useremail",
                Roles = new List<string> { "Guest" }
            };

            _userService.Setup(us => us.GetCurrentUserAsync()).ReturnsAsync(user);
            _userRepository.Setup(u => u.GetIdentityUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _blogPostRepository.Setup(bp => bp.UpdateBlogPost(It.IsAny<BlogPostDTO>())).ReturnsAsync(blogPost);

            _blogPostsController = new BlogPostsController(_blogPostRepository.Object, _blogPostService.Object,
                _userRepository.Object, _userService.Object, _dbContext, _categoryRepository.Object,
                _tagRepository.Object);

            // Act
            var result = await _blogPostsController.UpdateBlogPost(BlogPostBodyDTO.FromBlogPostDTO(blogPost));

            // Assert 
            Assert.IsType<UnauthorizedResult>(result);
        }

    }


}
