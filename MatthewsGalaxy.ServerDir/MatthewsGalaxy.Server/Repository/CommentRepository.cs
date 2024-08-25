using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CommentDTO>> GetComments()
        {
            return await _context.Comments.Select(c => CommentDTO.FromComment(c)).ToListAsync();
        }

        public async Task<CommentDTO> GetComment(Guid id)
        {
            // Step 1: Fetch the comment with related entities
            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.BlogPost)
                .FirstOrDefaultAsync(c => c.Id == id);

            // Step 2: Check if the comment is found
            if (comment == null)
            {
                return null; // or handle accordingly
            }

            // Step 3: Convert to CommentDTO
            return CommentDTO.FromComment(comment);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByBlogPost(Guid blogPostId)
        {
            List<CommentDTO>? comments = await _context.Comments
                .Where(c => c.BlogPostId == blogPostId)
                .Include(c => c.Author)
                .Include(c => c.BlogPost)
                .Select(c => CommentDTO.FromComment(c)).ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByUser(Guid userId)
        {
            return await _context.Comments
                .Where(c => c.AuthorId == userId.ToString())
                .Select(c => CommentDTO.FromComment(c)).ToListAsync();
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByUserName(string userName)
        {
            return await _context.Comments
                .Join(
                    _context.Users,
                    c => c.AuthorId,
                    u => u.Id,
                    (c, u) => new { Comment = c, Author = u }
                )
                .Where(cu => cu.Author.UserName.Contains(userName))
                .Select(cu => new CommentDTO
                {
                    Title = cu.Comment.Title,
                    Date = cu.Comment.Date,
                    Content = cu.Comment.Content,
                    AuthorId = cu.Comment.AuthorId,
                    ArticleId = cu.Comment.BlogPostId.ToString(),
                    ArticleHashID = cu.Comment.BlogPostId.GetHashCode().ToString(),
                    Id = cu.Comment.Id,
                    Author = new UserDTO()
                    {
                        Id = cu.Author.Id,
                        UserName = cu.Author.UserName,
                        Email = cu.Author.Email,
                        EmailConfirmed = cu.Author.EmailConfirmed
                    }
                }).ToListAsync();
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByDate(string date)
        {
            return await _context.Comments
                .Where(c => c.Date.ToString().Contains(date))
                .Select(c => CommentDTO.FromComment(c))
                .ToListAsync();
        }

        public async Task<CommentDTO> CreateComment(CommentDTO comment)
        {
            _context.Comments.Add(comment.ToComment());
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<CommentDTO> UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return CommentDTO.FromComment(comment);
        }

        public async Task<CommentDTO> DeleteComment(Guid id)
        {
            // Find the comment
            var comment = await _context.Comments
                .Include(c => c.Author) // Optional: Include related entities if needed
                .Include(c => c.BlogPost)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                // Handle case when comment is not found
                return null;
            }

            // Remove the comment from the context
            _context.Comments.Remove(comment);

            // Save changes to persist deletion
            await _context.SaveChangesAsync();

            return CommentDTO.FromComment(comment);
        }
    }
}
