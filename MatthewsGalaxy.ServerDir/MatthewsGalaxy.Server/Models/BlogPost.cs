using System.ComponentModel.DataAnnotations;
using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.Models.Users;

namespace MatthewsGalaxy.Server.Models
{
    public class BlogPost
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? FeaturedImageURL { get; set; }
        public string? UrlHandle { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public int Views { get; set; }
        [Required]
        public int Likes { get; set; }

        [Required]
        public required string AuthorId { get; set; }
        // Each blog post has an author that is a site admin (a blogger)
        public SiteAdmin? Author { get; set; }

        [Required]
        public bool IsVisible { get; set; }
        public bool IsFeatured { get; set; }
        
        // Each blog post has many comments
        public ICollection<Comment>? Comments { get; set; }
        // Each blog post belongs to many categories
        public ICollection<Category>? Categories { get; set; }

        // Each blog post has many tags
        public ICollection<PostTags>? PostTags { get; set; }
        // Each blog post can be liked by many users
        public ICollection<PostLikes>? PostLikes { get; set; }   
        // Each blog can have multiple categories
        public ICollection<PostCategories>? PostCategories { get; set; }

        // Each blog post can be saved by many guests
        public ICollection<Guest>? SavedBy { get; set; }




    }
}
