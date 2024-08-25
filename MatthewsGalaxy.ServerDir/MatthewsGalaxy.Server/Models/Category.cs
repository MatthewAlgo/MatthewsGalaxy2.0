using System.ComponentModel.DataAnnotations;

namespace MatthewsGalaxy.Server.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? UrlHandle { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }

        public ICollection<PostCategories>? PostCategories { get; set; }
    }
}
