using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface ITagRepository
    {
        Task <IEnumerable<TagDTO>> GetTags();
        Task <TagDTO> GetTag(Guid id);

        Task<IEnumerable<TagDTO>> GetTagsByBlogPost(Guid postId);
        Task<IEnumerable<TagDTO>> GetTagsByTagName(string tagName);

        Task <TagDTO> CreateTag(Tag tag);
        Task <TagDTO> UpdateTag(Tag tag);
        Task <TagDTO> DeleteTag(Guid id);

        Task <TagDTO> AddTagToBlogPost(Guid postId, string tagName);
        Task<TagDTO> RemoveTagFromBlogPost(Guid postId, string tagName);

        // Get tag by name
        Task<TagDTO> GetTagByName(string name);
    }
}
