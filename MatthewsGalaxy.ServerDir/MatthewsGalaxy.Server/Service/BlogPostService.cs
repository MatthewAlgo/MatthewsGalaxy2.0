using System.Diagnostics.Eventing.Reader;
using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Service
{
    public class BlogPostService : IBlogPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostService(ApplicationDbContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }
        // Like a blog post
        public async Task<BlogPostDTO> LikeBlogPost(string id, string username, string useremail)
        {
            // We need to find the article that has the hash as the id
            foreach (var bPost in _context.BlogPosts)
            {
                if (bPost.Id.GetHashCode().ToString() == id.ToString())
                {
                    id = bPost.Id.ToString();
                    break;
                }
            }

            var blogPost = await _context.BlogPosts.FindAsync(Guid.Parse(id));
            blogPost.Likes++;

            // Add the like to the post likes
            _context.PostLikes.Add(new PostLikes
            {
                BlogPostId = Guid.Parse(id),
                UserId = _context.Users.Where(u => u.UserName == username && u.Email == useremail).FirstOrDefault().Id
            });

            await _context.SaveChangesAsync();
            return BlogPostDTO.FromBlogPost(blogPost);
        }

        // Unlike a blog post
        public async Task<BlogPostDTO> UnlikeBlogPost(string id, string username, string useremail)
        {
            // We need to find the article that has the hash as the id
            foreach (var bPost in _context.BlogPosts)
            {
                if (bPost.Id.GetHashCode().ToString() == id.ToString())
                {
                    id = bPost.Id.ToString();
                    break;
                }
            }
            var blogPost = await _context.BlogPosts.FindAsync(Guid.Parse(id));
            blogPost.Likes--;

            // Remove the like from the post likes
            _context.PostLikes.RemoveRange(
                await _context.PostLikes.Where(pl => pl.BlogPostId.ToString() == id && pl.UserId == _context.Users.Where(u => u.UserName == username && u.Email == useremail).FirstOrDefault().Id
                ).ToListAsync()
            );

            await _context.SaveChangesAsync();
            return BlogPostDTO.FromBlogPost(blogPost);
        }

        public async Task<BlogPostDTO> IsLikedByUser(string id, string username, string useremail)
        {
            // We need to find the article that has the hash as the id
            foreach (var bPost in _context.BlogPosts)
            {
                if (bPost.Id.GetHashCode().ToString() == id)
                {
                    id = bPost.Id.ToString();
                    break;
                }
            }

            var blogPost = await _context.BlogPosts.FindAsync(Guid.Parse(id));
            var user = await _context.Users.Where(u => u.UserName == username && u.Email == useremail).FirstOrDefaultAsync();

            if (await _context.PostLikes.Where(pl => pl.BlogPostId.ToString() == id && pl.UserId == user.Id).FirstOrDefaultAsync() != null)
            {
                return BlogPostDTO.FromBlogPost(blogPost);
            }
            else
            {
                return null;
            }
        }

        public async Task<BlogPostDTO> ViewBlogPost(string id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(Guid.Parse(id));
            blogPost.Views++;
            await _context.SaveChangesAsync();
            return BlogPostDTO.FromBlogPost(blogPost);
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryId(Guid categoryId)
        {
            var blogPosts = await _context.PostCategories
                .Include(pc => pc.BlogPost)
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => BlogPostDTO.FromBlogPost(pc.BlogPost))
                .ToListAsync();
            return blogPosts;

        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryName(string categoryName) {
            var articles = await _context.PostCategories
                .Include(pc => pc.BlogPost)
                .Where(pc => pc.Category.Name.Equals(categoryName))
                .Select(pc => BlogPostDTO.FromBlogPost(pc.BlogPost))
                .ToListAsync();
            return articles;
        }
    }
}
