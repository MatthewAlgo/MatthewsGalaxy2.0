using System.ComponentModel.DataAnnotations;
using MatthewsGalaxy.Server.Data;

namespace MatthewsGalaxy.Server.Models.Users
{
    public class SiteAdmin : User
    {

        [System.ComponentModel.DataAnnotations.Required]
        public ICollection<BlogPost> BlogPostsPosted { get; set; }

    }
}
