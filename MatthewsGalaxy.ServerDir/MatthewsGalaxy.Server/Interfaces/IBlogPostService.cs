using MatthewsGalaxy.Server.DTOs;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IBlogPostService
    {
        Task<BlogPostDTO> LikeBlogPost(string id, string username, string useremail); // PostId, UserId
        Task<BlogPostDTO> UnlikeBlogPost(string id, string username, string useremail); // PostId, UserId
        Task<BlogPostDTO> IsLikedByUser
        (
            string id, // PostId
            string username, // UserId
            string useremail // UserEmail
        );

        Task<BlogPostDTO> ViewBlogPost (string id); // PostId
        Task<IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryId(Guid categoryId);
        Task <IEnumerable<BlogPostDTO>> GetBlogPostsByCategoryName(string searchTerm);
    }
}
