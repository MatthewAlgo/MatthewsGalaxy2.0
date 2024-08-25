using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatthewsGalaxy.Server.Models
{
    public class Tag
    {
        // Each post can have multiple tags
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string TagName { get; set; }

        public string? TagDescription { get; set; } // If a tag can have a description
        public string? TagIconURL { get; set; } // If a tag can have an icon

        // Each tag can have multiple blog posts
        [NotMapped]
        public ICollection<BlogPost>? BlogPosts { get; set; }
        public ICollection<PostTags>? PostTags { get; set; }
    }
}
