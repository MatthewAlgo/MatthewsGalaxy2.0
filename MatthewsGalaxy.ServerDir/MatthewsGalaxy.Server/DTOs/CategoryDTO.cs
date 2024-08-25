using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; }
        public string UrlHandle { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }

        public CategoryDTO(Category category)
        {
            Name = category.Name;
            if (category.UrlHandle == null)
            {
                UrlHandle = category.Name.Replace(" ", "-").ToLower();
            }
            else
            {
                UrlHandle = category.UrlHandle;
            }
            if (category.Description == null)
            {
                Description = "No description available.";
            }
            else
            {
                Description = category.Description;
            }
            if (category.ImageURL == null)
            {
                ImageURL = "https://via.placeholder.com/150";
            }
            else
            {
                ImageURL = category.ImageURL;
            }
        }

        public CategoryDTO() { }

        public Category ToCategory()
        {
            return new Category
            {
                Name = Name,
                UrlHandle = UrlHandle,
                Description = Description,
                ImageURL = ImageURL
            };
        }

        public Category ToCategory(Category category)
        {
            category.Name = Name;
            category.UrlHandle = UrlHandle;
            category.Description = Description;
            category.ImageURL = ImageURL;
            return category;
        }

        public static CategoryDTO FromCategory(Category category)
        {
            return new CategoryDTO(category);
        }

        public static Category FromCategoryDTO(CategoryDTO categoryDTO)
        {
            return categoryDTO.ToCategory();
        }
    }
}
