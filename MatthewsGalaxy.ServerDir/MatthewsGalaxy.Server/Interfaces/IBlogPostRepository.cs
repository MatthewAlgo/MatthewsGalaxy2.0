using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPostDTO> GetBlogPost(string id);
        Task<IEnumerable<BlogPostDTO>> GetBlogPosts();
        Task<IEnumerable<BlogPost>> GetBlogPostsRaw();
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsBySearchTerm(string searchTerm);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByTagId(string tagId);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByAuthor(string author);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByDate(string date);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByViews(int views);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByLikes(int likes);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByComments(int comments);
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByVisible(bool visible);
        Task<BlogPostDTO> CreateBlogPost(BlogPostDTO blogPost);
        Task<BlogPostDTO> UpdateBlogPost(BlogPostDTO blogPost);
        Task<BlogPostDTO> DeleteBlogPost(string id);

    }
}
