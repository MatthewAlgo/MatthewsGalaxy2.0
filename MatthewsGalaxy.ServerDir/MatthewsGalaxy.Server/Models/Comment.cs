using System.ComponentModel.DataAnnotations;
using MatthewsGalaxy.Server.Data;

namespace MatthewsGalaxy.Server.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        // Each comment has an author
        [Required]
        public string AuthorId { get; set; }
        public User Author { get; set; }
        
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }

        // Each comment belongs to a blog post
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
