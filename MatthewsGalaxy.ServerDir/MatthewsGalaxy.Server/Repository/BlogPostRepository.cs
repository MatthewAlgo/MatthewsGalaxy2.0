using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Repository
{
    // A repository for blog posts, it only features database operations
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlogPostService _blogPostService;
        private readonly ITagRepository _tagRepository;

        public BlogPostRepository(ApplicationDbContext context, ICategoryRepository categoryRepository, IBlogPostService blogPostService,
            ITagRepository tagRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _blogPostService = blogPostService;
            _tagRepository = tagRepository;
        }
        public async Task<IEnumerable<BlogPostDTO>> GetBlogPosts()
        {
            return await _context.BlogPosts.Select(bp 
                => BlogPostDTO.FromBlogPost(bp)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsRaw() => await _context.BlogPosts.ToListAsync();

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsBySearchTerm(string searchTerm)
        {
            return await _context.BlogPosts
                .Where(bp => bp.Title.Contains(searchTerm) || bp.ShortDescription.Contains(searchTerm) || bp.Description.Contains(searchTerm))
                .Select(bp => BlogPostDTO.FromBlogPost(bp)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByTagId(string tagId)
        {
            return await _context.BlogPosts
                .Include("PostTags")
                .Where(bp => bp.PostTags.Any(t => t.TagId.ToString() == tagId))
                .Select(bp => BlogPostDTO.FromBlogPost(bp)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByAuthor(string author)
        {
            return await _context.BlogPosts
                .Join( // Join the blog posts with the users
                    _context.Users,
                    bp => bp.AuthorId,
                    u => u.Id,
                    (bp, u) => new { bp, u }
                )
                .Where(bp => bp.u.UserName.Contains(author))
                .Select(bp => BlogPostDTO.FromBlogPostWithAuthor(
                    bp.bp, bp.u.UserName)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByDate(string date)
        {
            IEnumerable<BlogPost> list = await _context.BlogPosts
                .Where(bp => bp.PublishedDate.ToString().Contains(date))
                .OrderBy(bp => bp.Id.GetHashCode()).ToListAsync();
            return list.Select(bp => BlogPostDTO.FromBlogPost(bp)).ToList();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByViews(int views)
        {
            return await _context.BlogPosts
                .Where(bp => bp.Views == views)
                .Select(bp => BlogPostDTO.FromBlogPost(bp)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByLikes(int likes)
        {
            return await _context.BlogPosts
                .Where(bp => bp.Likes == likes)
                .Select(bp => BlogPostDTO.FromBlogPost(bp)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByComments(int comments)
        {
            return await _context.BlogPosts
                .Where(bp => bp.Comments.Count == comments)
                .Select(bp => BlogPostDTO.FromBlogPost(bp)).ToListAsync();
        }

        public async Task<IEnumerable<BlogPostDTO>> GetBlogPostsByVisible(bool visible)
        {
            return await Task.FromResult(_context.BlogPosts
                           .Where(bp => bp.IsVisible == visible)
                           .Select(bp => BlogPostDTO.FromBlogPost(bp)).ToList());
        }

        public async Task<BlogPostDTO> GetBlogPost(string id)
        {
            var blogPost = await _context.BlogPosts.Where(bp => bp.Id.ToString() == id).FirstOrDefaultAsync();
            return BlogPostDTO.FromBlogPostWithAuthor(
                blogPost, 
                (await _context.Users.FindAsync(blogPost.AuthorId)).UserName);
        }

        public async Task<BlogPostDTO> CreateBlogPost(BlogPostDTO blogPostDTO)
        {
            var blogPost = blogPostDTO.ToBlogPost();
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();
            return BlogPostDTO.FromBlogPost(blogPost);
        }

        public Task<BlogPostDTO> UpdateBlogPost(BlogPostDTO blogPost)
        {
            var blogPostEntity = blogPost.ToBlogPost();
            _context.BlogPosts.Update(blogPostEntity);
            return Task.FromResult(BlogPostDTO.FromBlogPost(blogPostEntity));
        }

        public async Task<BlogPostDTO> DeleteBlogPost(string id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(Guid.Parse(id));
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            // Also remove the post comments
            var comments = await _context.Comments.Where(c => c.BlogPostId.ToString() == id).ToListAsync();
            _context.Comments.RemoveRange(comments);
            await _context.SaveChangesAsync();

            // Also remove the post categories and tags
            var postCategories = await _context.PostCategories.Where(pc => pc.BlogPostId.ToString() == id).ToListAsync();
            _context.PostCategories.RemoveRange(postCategories);
            await _context.SaveChangesAsync();

            var postTags = await _context.PostTags.Where(pt => pt.BlogPostId.ToString() == id).ToListAsync();
            _context.PostTags.RemoveRange(postTags);
            await _context.SaveChangesAsync();

            // Remove all categories and tags that are not used
            var categories = await _context.Categories.Where(c => c.PostCategories.Count == 0).ToListAsync();
            _context.Categories.RemoveRange(categories);
            await _context.SaveChangesAsync();

            var tags = await _context.Tags.Where(t => t.PostTags.Count == 0).ToListAsync();
            _context.Tags.RemoveRange(tags);
            await _context.SaveChangesAsync();

            return BlogPostDTO.FromBlogPost(blogPost);
        }


    }
}
