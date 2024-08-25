using System.ComponentModel.DataAnnotations;
using MatthewsGalaxy.Server.Data;

namespace MatthewsGalaxy.Server.Models
{
    public class PostLikes
    {
        // Composed key out of the post ID and the user ID
        [Required]
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}