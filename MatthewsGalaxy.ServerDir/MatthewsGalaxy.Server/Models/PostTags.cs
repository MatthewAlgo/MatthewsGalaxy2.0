using Microsoft.Build.Framework;

namespace MatthewsGalaxy.Server.Models
{
    public class PostTags
    {
        // Composed key out of the post ID and the tag ID
        [Required]
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        [Required]
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
