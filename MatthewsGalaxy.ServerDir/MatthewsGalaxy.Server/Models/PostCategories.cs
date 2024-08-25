using System.ComponentModel.DataAnnotations;

namespace MatthewsGalaxy.Server.Models
{
    public class PostCategories
    {
        // Composed key out of the post ID and the category ID
        [Required]
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
