using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategory(Guid id);
        Task<CategoryDTO> GetCategory(string urlHandle);
        Task<CategoryDTO> CreateCategory(CategoryDTO category);
        Task<CategoryDTO> UpdateCategory(Category category);
        Task<CategoryDTO> DeleteCategory(Guid id);
        // Get category by name
        Task<CategoryDTO> GetCategoryByName(string name);
        // Get category by description
        Task<CategoryDTO> GetCategoryByDescription(string description);
        // Get category by blog post
        Task<IEnumerable<CategoryDTO>> GetCategoriesByArticleId(string blogPostId);
        Task<CategoryDTO> AddCategoryToArticle(Guid articleId, string categoryName);
        Task<CategoryDTO> RemoveCategoryFromArticle(Guid articleId, string categoryName);
    }
}
