using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class BlogPostDTO
    {
        public string ComputedHashId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public bool IsVisible { get; set; }
        public bool IsFeatured { get; set; }
        public string FeaturedImageURL { get; set; }
        public string UrlHandle { get; set; }

        public BlogPostDTO(BlogPost blogPost)
        {
            // If the blogpost is not null 
            if (blogPost == null) return;
            ComputedHashId = blogPost.Id.GetHashCode().ToString();
            Title = blogPost.Title;
            ShortDescription = blogPost.ShortDescription;
            Description = blogPost.Description;
            PublishedDate = blogPost.PublishedDate;
            Views = blogPost.Views;
            Likes = blogPost.Likes;
            AuthorId = blogPost.AuthorId;
            IsVisible = blogPost.IsVisible;
            IsFeatured = blogPost.IsFeatured;
            FeaturedImageURL = blogPost.FeaturedImageURL;
            UrlHandle = blogPost.UrlHandle;
        }

        public BlogPostDTO() { }

        public BlogPost ToBlogPost()
        {
            return new BlogPost
            {
                Title = Title,
                ShortDescription = ShortDescription,
                Description = Description,
                PublishedDate = PublishedDate,
                Views = Views,
                Likes = Likes,
                AuthorId = AuthorId,
                IsVisible = IsVisible,
                IsFeatured = IsFeatured,
                FeaturedImageURL = FeaturedImageURL,
                UrlHandle = UrlHandle,
            };
        }

        public BlogPost ToBlogPost(BlogPost blogPost)
        {
            blogPost.Title = Title;
            blogPost.ShortDescription = ShortDescription;
            blogPost.Description = Description;
            blogPost.PublishedDate = PublishedDate;
            blogPost.Views = Views;
            blogPost.Likes = Likes;
            blogPost.AuthorId = AuthorId;
            blogPost.IsVisible = IsVisible;
            blogPost.IsFeatured = IsFeatured;
            blogPost.FeaturedImageURL = FeaturedImageURL;
            blogPost.UrlHandle = UrlHandle;
            if (blogPost.Author != null)
            {
                blogPost.Author.UserName = AuthorName;
            }
            return blogPost;
        }

        public static BlogPostDTO FromBlogPost(BlogPost blogPost)
        {
            return new BlogPostDTO(blogPost);
        }

        public static BlogPost FromBlogPostDTO(BlogPostDTO blogPostDTO)
        {
            return blogPostDTO.ToBlogPost();
        }

        public static BlogPostDTO FromBlogPostWithAuthor(BlogPost blogPost, string authorName)
        {
            var blogPostDTO = new BlogPostDTO(blogPost);
            blogPostDTO.AuthorName = authorName;
            return blogPostDTO;
        }

    }
}
