using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class BlogPostBodyDTO
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string FeaturedImageURL { get; set; }
        public string ShortDescription { get; set; }

        public BlogPostBodyDTO(BlogPost blogPost)
        {
            Description = blogPost.Description;
            Title = blogPost.Title;
            FeaturedImageURL = blogPost.FeaturedImageURL;
            ShortDescription = blogPost.ShortDescription;
        }

        public BlogPostBodyDTO() { }

        public BlogPostDTO ToBlogPostDTO()
        {
            return new BlogPostDTO
            {
                Description = Description,
                Title = Title,
                FeaturedImageURL = FeaturedImageURL,
                ShortDescription = ShortDescription
            };
        }

        public BlogPost ToBlogPost(BlogPost blogPost)
        {
            blogPost.Description = Description;
            blogPost.Title = Title;
            blogPost.FeaturedImageURL = FeaturedImageURL;
            blogPost.ShortDescription = ShortDescription;
            return blogPost;
        }

        public static BlogPostBodyDTO FromBlogPost(BlogPost blogPost)
        {
            return new BlogPostBodyDTO(blogPost);
        }

        public static BlogPostBodyDTO FromBlogPostDTO(BlogPostDTO blogPostDTO)
        {
            return new BlogPostBodyDTO
            {
                Description = blogPostDTO.Description,
                Title = blogPostDTO.Title,
                FeaturedImageURL = blogPostDTO.FeaturedImageURL,
                ShortDescription = blogPostDTO.ShortDescription
            };
        }

    }
}
