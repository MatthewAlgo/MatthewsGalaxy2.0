using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.Server.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentsController> _logger;
        // Insert the user repository here
        private readonly IUserRepository _userRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserService _userService;


        public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository,
            ILogger<CommentsController> logger, IBlogPostRepository blogPostRepository,
            IUserService userService)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _logger = logger;
            _blogPostRepository = blogPostRepository;
            _userService = userService;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))] // OK
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentRepository.GetComments();
            if (ModelState.IsValid)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get all comments of blog post
        [HttpGet]
        [Route("article/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))] // OK
        public async Task<IActionResult> GetCommentsByBlogPost(string id)
        {
            // The blog post ID is a hash of the actual ID. So we need to get the actual ID first
            User user = await _userService.GetCurrentUserAsync();
            foreach (var blogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (blogPost.Id.GetHashCode().ToString() == id.ToString())
                {
                    id = blogPost.Id.ToString();
                    break;
                }
            }

            var comments = await _commentRepository.GetCommentsByBlogPost(
                Guid.Parse(id));

            // For each comment, determine if it is the current user's comment
            foreach (var comment in comments)
            {
                if ((user != null && comment.AuthorId == user.Id) || (user != null && user.Roles[0] == "Admin"))
                {
                    comment.IsLoggedUsersComment = true;
                }
            }

            if (ModelState.IsValid)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("search/{id}")]
        [ProducesResponseType(200, Type = typeof(Comment))] // OK
        public async Task<IActionResult> GetComment(Guid id)
        {
            var comment = await _commentRepository.GetComment(id);
            if (ModelState.IsValid)
            {
                return Ok(comment);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("search_post/{postId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))] // OK
        public async Task<IActionResult> GetComments(Guid postId)
        {
            var comments = await _commentRepository.GetCommentsByBlogPost(postId);
            if (ModelState.IsValid)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("search_user/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))] // OK
        public async Task<IActionResult> GetCommentsByUser(Guid userId)
        {
            var comments = await _commentRepository.GetCommentsByUser(userId);
            if (ModelState.IsValid)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("search_username/{userName}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))] // OK
        public async Task<IActionResult> GetCommentsByUserName(string userName)
        {
            var comments = await _commentRepository.GetCommentsByUserName(userName);
            if (ModelState.IsValid)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("search_date/{date}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))] // OK
        public async Task<IActionResult> GetCommentsByDate(string date)
        {
            var comments = await _commentRepository.GetCommentsByDate(date);
            if (ModelState.IsValid)
            {
                return Ok(comments);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("add/{articleId}/{username}/{useremail}")]
        [ProducesResponseType(201, Type = typeof(Comment))] // Created
        public async Task<IActionResult> CreateComment(string articleId, string username, string useremail,
            [FromBody] CommentBodyDTO comment)
        {
            // Find the blog post by ID
            foreach (var blogPost in await _blogPostRepository.GetBlogPostsRaw())
            {
                if (blogPost.Id.GetHashCode().ToString() == articleId.ToString())
                {
                    articleId = blogPost.Id.ToString();
                    break;
                }
            }

            // Validate comment content
            var content = comment.GetContent();

            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError(nameof(comment.GetContent), "Content cannot be empty.");
                return BadRequest(ModelState);
            }

            if (content.Length > 500)
            {
                ModelState.AddModelError(nameof(comment.GetContent), "Content cannot be longer than 500 characters.");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(comment.GetTitle()))
            {
                ModelState.AddModelError(nameof(comment.GetTitle), "Title cannot be empty.");
                return BadRequest(ModelState);
            }

            if (comment.GetTitle().Length > 100)
            {
                ModelState.AddModelError(nameof(comment.GetTitle), "Title cannot be longer than 100 characters.");
                return BadRequest(ModelState);
            }

            // If the user has made more than 1 comment, return an error
            var commentsWithAuthors = await _commentRepository.GetCommentsByUserName(username);
            foreach (var commentWithAuthor in commentsWithAuthors)
            {
                if (commentWithAuthor.Author.UserName == username && commentWithAuthor.Author.Email == useremail &&
                    commentWithAuthor.ArticleId == articleId)
                {
                    {
                        ModelState.AddModelError(nameof(comment.GetContent),
                            "You have already commented on this article.");
                        return BadRequest(ModelState);
                    }
                }
            }

            // Get the user by username and email
            User? user = await _userRepository.GetIdentityUser(username, useremail);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var newComment = new CommentDTO()
            {

                Title = comment.GetTitle(),
                Content = comment.GetContent(),
                AuthorId = user.Id,
                ArticleId = articleId,
                ArticleHashID = articleId,
                Date = DateTime.UtcNow,
                Id = Guid.NewGuid()
            };

            if (ModelState.IsValid)
            {
                await _commentRepository.CreateComment(newComment);
                // Ensure you provide all necessary route values here
                return CreatedAtAction(nameof(GetComment), new { id = newComment.Id }, newComment);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType(201, Type = typeof(Comment))] // Created
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            var newComment = await _commentRepository.CreateComment(CommentDTO.FromComment(comment));
            if (ModelState.IsValid)
            {
                return CreatedAtAction(nameof(GetComment), newComment);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Comment))] // OK
        public async Task<IActionResult> UpdateComment([FromBody] Comment comment)
        {
            var updatedComment = await _commentRepository.UpdateComment(comment);
            if (ModelState.IsValid)
            {
                return Ok(updatedComment);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{id}")]
        [ProducesResponseType(200, Type = typeof(Comment))] // OK
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            // Get the comment with the specified ID
            var local_comment = await _commentRepository.GetComment(id);
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null || (local_comment.AuthorId != currentUser.Id && currentUser.Roles[0] != "Admin"))
            {
                return Unauthorized();
            }

            var deleted_comm = await _commentRepository.DeleteComment(id);
            if (ModelState.IsValid)
            {
                return Ok(deleted_comm);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
