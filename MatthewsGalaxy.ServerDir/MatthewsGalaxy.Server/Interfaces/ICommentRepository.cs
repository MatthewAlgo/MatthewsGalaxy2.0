using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentDTO>> GetComments();
        Task<CommentDTO> GetComment(Guid id);
        Task<IEnumerable<CommentDTO>> GetCommentsByBlogPost(Guid blogPostId);
        Task<IEnumerable<CommentDTO>> GetCommentsByUser(Guid userId);
        Task<IEnumerable<CommentDTO>> GetCommentsByUserName(string userName);
        Task<IEnumerable<CommentDTO>> GetCommentsByDate(string date);
        Task<CommentDTO> CreateComment(CommentDTO comment);
        Task<Comment> CreateComment(Comment comment);
        Task<CommentDTO> UpdateComment(Comment comment);
        Task<CommentDTO> DeleteComment(Guid id);

    }
}
