using System.ComponentModel.DataAnnotations;
using MatthewsGalaxy.Server.Data;

namespace MatthewsGalaxy.Server.Models.Users
{
    public class Guest : User
    {

        [Required]
        public BlogPost[] SavedBlogPosts { get; set; }
    }
}
