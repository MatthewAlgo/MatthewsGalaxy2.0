using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.Server.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostService _blogPostService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, IBlogPostService blogPostService, IUserRepository userRepository, IUserService userService, ApplicationDbContext context, 
            ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostService = blogPostService;
            _userRepository = userRepository;
            _userService = userService;
            _context = context;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetBlogPosts();
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}/likes")]
        [ProducesResponseType(200, Type = typeof(int))] // OK
        public async Task<IActionResult> GetBlogPostLikes(string id)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }
            if (ModelState.IsValid)
            {
                return Ok(_context.PostLikes.Where(x => x.BlogPostId.ToString() == id).Count());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get blog posts by search term
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsBySearchTerm(string searchTerm)
        {
            var blogPosts = await _blogPostRepository.GetBlogPostsBySearchTerm(searchTerm);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get blog posts by tag
        [HttpGet("tag/{tagId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByTagId(string tagId)
        {
            var blogPosts = await _blogPostRepository.GetBlogPostsByTagId(tagId);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get blog posts by author
        [HttpGet("author/{author}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByAuthor(string author)
        {
            var blogPosts = await _blogPostRepository.GetBlogPostsByAuthor(author);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        // Get blog posts by date
        [HttpGet("date/{date}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByDate(string date)
        {
            var blogPosts = await _blogPostRepository.GetBlogPostsByDate(date);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(BlogPost))] // OK
        public async Task<IActionResult> GetBlogPost(string id)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id.ToString())
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var blogPost = await _blogPostRepository.GetBlogPost(id.ToString());
            if (ModelState.IsValid)
            {
                return Ok(blogPost);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Like and unlike blog post
        [Authorize]
        [HttpPost("like/{id}/{username}/{useremail}")]
        [ProducesResponseType(200, Type = typeof(BlogPost))] // OK
        public async Task<IActionResult> LikeBlogPost(string id, string username, string useremail)
        {
            // Find if the provided user exists
            User? user = await _userService.GetCurrentUserAsync();
            // If the name and email do not match the user, return unauthorized
            if (user==null || user.UserName != username || user.Email != useremail)
            {
                return Unauthorized();
            }
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id.ToString())
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var blogPost = await _blogPostService.LikeBlogPost(id, username, useremail);
            if (ModelState.IsValid)
            {
                return Ok(blogPost);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpPost("unlike/{id}/{username}/{useremail}")]
        [ProducesResponseType(200, Type = typeof(BlogPost))] // OK
        public async Task<IActionResult> UnlikeBlogPost(string id, string username, string useremail)
        {
            // Find if the provided user exists
            User? user = await _userService.GetCurrentUserAsync();
            // If the name and email do not match the user, return unauthorized
            if (user == null || user.UserName != username || user.Email != useremail)
            {
                return Unauthorized();
            }

            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id.ToString())
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var blogPost = await _blogPostService.UnlikeBlogPost(id, username, useremail);
            if (ModelState.IsValid)
            {
                return Ok(blogPost);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Is liked by user
        [Authorize]
        [HttpGet("isliked/{id}/{username}/{useremail}")]
        [ProducesResponseType(200, Type = typeof(bool))] // OK
        public async Task<IActionResult> IsLikedByUser(string id, string username, string useremail)
        {
            try
            {
                var result = await _blogPostService.IsLikedByUser(id, username, useremail);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("The post is not liked by the user or the post/user does not exist.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Controllers for Admins
        [HttpGet("views/{views}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByViews(int views)
        {
            User? currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser==null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var blogPosts = await _blogPostRepository.GetBlogPostsByViews(views);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("likes/{likes}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByLikes(int likes)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser==null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var blogPosts = await _blogPostRepository.GetBlogPostsByLikes(likes);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("comments/{comments}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByComments(int comments)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var blogPosts = await _blogPostRepository.GetBlogPostsByComments(comments);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet("visible/{visible}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByPublished(bool visible)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var blogPosts = await _blogPostRepository.GetBlogPostsByVisible(visible);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("incrementViews/{postId}")]
        [ProducesResponseType (200, Type = typeof(BlogPost))] // OK
        public async Task<IActionResult> ViewPost(string postId)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == postId)
                {
                    postId = elblogPost.Id.ToString();
                    break;
                }
            }

            var blogPost = await _blogPostService.ViewBlogPost(postId);
            if (ModelState.IsValid)
            {
                return Ok(blogPost);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(201, Type = typeof(BlogPost))] // Created
        public async Task<IActionResult> CreateBlogPost([FromBody] BlogPostBodyDTO blogPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var localBlogPostObject = new BlogPost
            {
                Id = Guid.NewGuid(),
                AuthorId = currentUser.Id,
                Description = blogPost.Description,
                Title = blogPost.Title,
                FeaturedImageURL = blogPost.FeaturedImageURL,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = DateTime.UtcNow,
                Views = 0,
                Likes = 0,
                Comments = new List<Comment>(),
                IsVisible = true,
                Author = new SiteAdmin
                {
                    Id = currentUser.Id,
                    UserName = currentUser.UserName,
                    Email = currentUser.Email,
                    Roles = currentUser.Roles
                },
                IsFeatured = false
            };

            var newBlogPost = await _blogPostRepository.CreateBlogPost(BlogPostDTO.FromBlogPost(localBlogPostObject));

            if (newBlogPost == null)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            
            return CreatedAtAction(nameof(GetBlogPost), new { id = localBlogPostObject.Id }, newBlogPost);
        }


        [HttpPut]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(BlogPost))] // OK
        public async Task<IActionResult> UpdateBlogPost([FromBody] BlogPostBodyDTO blogPost)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var updatedBlogPost = await _blogPostRepository.UpdateBlogPost(blogPost.ToBlogPostDTO());
            if (ModelState.IsValid)
            {
                return Ok(updatedBlogPost);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(BlogPost))] // OK
        public async Task<IActionResult> DeleteBlogPost(string id)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var blogPost = await _blogPostRepository.DeleteBlogPost(id);
            if (ModelState.IsValid)
            {
                return Ok(blogPost);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{id}/tags/{tagName}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Tag))] // OK
        public async Task<IActionResult> AddTagToBlogPost(string id, string tagName)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var postTag = await _tagRepository.AddTagToBlogPost(Guid.Parse(id), tagName);
            if (ModelState.IsValid)
            {
                return Ok(postTag);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}/tags/{tagName}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Tag))] // OK
        public async Task<IActionResult> RemoveTagFromBlogPost(string id, string tagName)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var postTag = await _tagRepository.RemoveTagFromBlogPost(Guid.Parse(id), tagName);
            if (ModelState.IsValid)
            {
                return Ok(postTag);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{id}/categories/{categoryName}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> AddCategoryToBlogPost(string id, string categoryName)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.AddCategoryToArticle
                (Guid.Parse(id), categoryName);
            if (ModelState.IsValid)
            {
                return Ok(category);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}/categories/{categoryName}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> RemoveCategoryFromBlogPost(string id, string categoryName)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.RemoveCategoryFromArticle
                (Guid.Parse(id), categoryName);
            if (ModelState.IsValid)
            {
                return Ok(category);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get categories of article
        [HttpGet("{id}/categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))] // OK
        public async Task<IActionResult> GetCategoriesOfBlogPost(string id)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var categories = await _categoryRepository .GetCategoriesByArticleId(id);
            if (ModelState.IsValid)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get tags of article
        [HttpGet("{id}/tags")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))] // OK
        public async Task<IActionResult> GetTagsOfBlogPost(string id)
        {
            // The id received is the hash of the ID of the blog post, determine the actual ID
            foreach (var elblogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (elblogPost.Id.GetHashCode().ToString() == id)
                {
                    id = elblogPost.Id.ToString();
                    break;
                }
            }

            var tags = await _tagRepository.GetTagsByBlogPost(Guid.Parse(id));
            if (ModelState.IsValid)
            {
                return Ok(tags);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get blog posts by category id
        [HttpGet ("categoryid/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByCategoryId(string categoryId)
        {
            var blogPosts = await _blogPostService.GetBlogPostsByCategoryId(Guid.Parse(categoryId));
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get posts by category name
        [HttpGet("categoryname/{categoryName}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogPost>))] // OK
        public async Task<IActionResult> GetBlogPostsByCategoryName(string categoryName)
        {
            var blogPosts = await _blogPostService.GetBlogPostsByCategoryName(categoryName);
            if (ModelState.IsValid)
            {
                return Ok(blogPosts);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
