using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            
            return await _context.Categories.Select(c => CategoryDTO.FromCategory(c)).ToListAsync();
        }

        public async Task<CategoryDTO> GetCategory(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(
                c => c.Id == id).ContinueWith(t => CategoryDTO.FromCategory(t.Result));
        }

        public async Task<CategoryDTO> GetCategory(string urlHandle)
        {
            return await _context.Categories.Select(
                c => CategoryDTO.FromCategory(c)).FirstOrDefaultAsync(c => c.UrlHandle.Contains(urlHandle));
        }

        public async Task<CategoryDTO> CreateCategory(CategoryDTO category)
        {
            _context.Categories.Add(category.ToCategory());
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<CategoryDTO> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return CategoryDTO.FromCategory(category);
        }

        public async Task<CategoryDTO> DeleteCategory(Guid id)
        {
            CategoryDTO category = CategoryDTO.FromCategory(
                               _context.Categories.Find(id));
            _context.Categories.Remove(category.ToCategory());
            await _context.SaveChangesAsync();
            return category;
        }

        // Get category by name
        public async Task<CategoryDTO> GetCategoryByName(string name)
        {
            return await _context.Categories.Select(
                c => CategoryDTO.FromCategory(c)).FirstOrDefaultAsync(c => c.Name.Contains(name));
        }

        // Get category by description
        public async Task<CategoryDTO> GetCategoryByDescription(string description)
        {
            return await _context.Categories.Select(
                c => CategoryDTO.FromCategory(c)).FirstOrDefaultAsync(c => c.Description.Contains(description));
        }

        // Get category by articleId
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesByArticleId (string blogPostId)
        {
            return await _context.PostCategories
                .Include(pc => pc.Category)
                .Where(pc => pc.BlogPostId.ToString() == blogPostId)
                .Select(pc => CategoryDTO.FromCategory(pc.Category))
                .ToListAsync();
        }

        // Add category to article given the article ID
        public async Task<CategoryDTO> AddCategoryToArticle(Guid articleId, string categoryName)
        {
            // If the article already has the category, return the category
            if (await _context.PostCategories.AnyAsync(pc => pc.BlogPostId == articleId &&
                pc.Category.Name == categoryName))
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName)
                    .ContinueWith(t => CategoryDTO.FromCategory(t.Result));
            }

            // If the category exists, add the category to the article
            if (await _context.Categories.AnyAsync(c => c.Name == categoryName))
            {
                var categoryLocal = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                var postCategoryLocal = new PostCategories()
                {
                    BlogPostId = articleId,
                    CategoryId = categoryLocal.Id
                };
                _context.PostCategories.Add(postCategoryLocal);
                await _context.SaveChangesAsync();
                return CategoryDTO.FromCategory(categoryLocal);
            }

            // If the category does not exist, create the category and add it to the article
            var category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = categoryName,
            };

            var postCategory = new PostCategories()
            {
                BlogPostId = articleId,
                CategoryId = category.Id
            };

            _context.Categories.Add(category);
            _context.PostCategories.Add(postCategory);
            await _context.SaveChangesAsync();
            return CategoryDTO.FromCategory(await _context.Categories.FindAsync(category.Id));
        }

        // Remove category from article given the article ID
        public async Task<CategoryDTO> RemoveCategoryFromArticle(Guid articleId, string categoryName)
        {
            // If the article does not have the category, return null
            if (!await _context.PostCategories.AnyAsync(pc => pc.BlogPostId == articleId &&
                pc.Category.Name == categoryName))
            {
                return null;
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            var postCategory = await _context.PostCategories.FirstOrDefaultAsync(pc => pc.BlogPostId == articleId &&
                pc.CategoryId == category
                    .Id);
            _context.PostCategories.Remove(postCategory);
            await _context.SaveChangesAsync();

            // If the category is not used by any other article, remove the category
            if (!await _context.PostCategories.AnyAsync(pc => pc.CategoryId == category.Id))
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return CategoryDTO.FromCategory(category);
        }
    }
}
