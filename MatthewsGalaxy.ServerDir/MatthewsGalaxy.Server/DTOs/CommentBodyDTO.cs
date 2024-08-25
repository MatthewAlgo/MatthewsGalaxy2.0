using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class CommentBodyDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public CommentBodyDTO() { }

        public CommentBodyDTO(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string GetTitle()
        {
            return Title;
        }

        public string GetContent()
        {
            return Content;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetContent(string content)
        {
            Content = content;
        }

        // From the DTO to the model
        public Comment ToComment()
        {
            return new Comment
            {
                Title = Title,
                Content = Content
            };
        }

        // From the model to the DTO
        public static CommentBodyDTO FromComment(Comment comment)
        {
            return new CommentBodyDTO
            {
                Title = comment.Title,
                Content = comment.Content
            };
        }
    }
}