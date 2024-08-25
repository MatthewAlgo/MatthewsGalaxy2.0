using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace MatthewsGalaxy.Server.Data
{
    public class User : IdentityUser
    {
        [NotMapped]
        public IList<string>? Roles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
        public ICollection<PostLikes> PostLikes { get; set; }
    }
}
