using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string ArticleId { get; set; }
        public string ArticleHashID { get; set; }
        public string commentHash { get; set; }
        public string AuthorId { get; set; }
        public UserDTO Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public BlogPostDTO BlogPost { get; set; }
        public Boolean IsLoggedUsersComment { get; set; }

        public CommentDTO(Comment comment)
        {
            Content = comment.Content;
            Date = comment.Date;
            Title = comment.Title;
            if (comment.BlogPost != null)
            {
                ArticleId = comment.BlogPost.Id.ToString();
                BlogPost = BlogPostDTO.FromBlogPost(comment.BlogPost);
                ArticleHashID = comment.BlogPost.Id.GetHashCode().ToString();
            }

            commentHash = comment.Id.GetHashCode().ToString();

            if (comment.Author != null)
            {
                AuthorId = comment.AuthorId;
                Author = new UserDTO(comment.Author);
            }
            IsLoggedUsersComment = false;
            Id = comment.Id;
        }

        public CommentDTO() { }

        public Comment ToComment()
        {
            return new Comment
            {
                Content = Content,
                Date = Date,
                Title = Title,
                Id = Id,
                AuthorId = AuthorId,
                BlogPostId = Guid.Parse(ArticleId)
            };
        }

        public Comment ToComment(Guid articleId)
        {
            return new Comment
            {
                Content = Content,
                Date = Date,
                Title = Title,
                BlogPostId = articleId,
                Id = Id,
                AuthorId = AuthorId,
            };
        }

        public Comment ToComment(Comment comment)
        {
            comment.Author = Author.ToUser();
            comment.Content = Content;
            comment.Date = Date;
            comment.Title = Title;
            comment.BlogPost = BlogPost.ToBlogPost();
            comment.Id = Id;
            comment.AuthorId = AuthorId;
            return comment;
        }

        public static CommentDTO FromComment(Comment comment)
        {
            return new CommentDTO(comment);
        }

        public static Comment FromCommentDTO(CommentDTO commentDTO)
        {
            return commentDTO.ToComment();
        }

    }
}
