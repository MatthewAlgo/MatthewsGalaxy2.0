using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagDTO>> GetTags()
        {
            return await _context.PostTags
                .Include(pt => pt.Tag)
                .Select(pt => TagDTO.FromPostTag(pt.Tag))
                .ToListAsync();
        }

        public async Task<TagDTO> GetTag(Guid id)
        {
            var tag = await _context.PostTags
                .Include(pt => pt.Tag)
                .Where(pt => pt.TagId == id)
                .Select(pt => pt.Tag)
                .FirstOrDefaultAsync();

            return TagDTO.FromPostTag(tag);
        }

        public async Task<IEnumerable<TagDTO>> GetTagsByBlogPost(Guid blogPostId)
        {
            return await _context.PostTags
                .Include(pt => pt.Tag)
                .Where(pt => pt.BlogPostId == blogPostId)
                .Select(pt => TagDTO.FromPostTag(pt.Tag))
                .ToListAsync();
        }

        public async Task<IEnumerable<TagDTO>> GetTagsByTagName(string tagName)
        {
            return await _context.PostTags
                .Include(pt => pt.Tag)
                .Where(pt => pt.Tag.TagName.Contains(tagName))
                .Select(pt => TagDTO.FromPostTag(pt.Tag))
                .ToListAsync();
        }

        public async Task<TagDTO> CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return TagDTO.FromPostTag(tag);
        }

        public async Task<TagDTO> UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return TagDTO.FromPostTag(tag);
        }

        public async Task<TagDTO> DeleteTag(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
            return TagDTO.FromPostTag(tag);
        }

        public async Task<TagDTO> AddTagToBlogPost(Guid postId, string tagName)
        {
            // Try to find the tag by name
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);

            // If the tag does not exist, create it
            if (tag == null)
            {
                tag = new Tag
                {
                    TagName = tagName,
                    Id = Guid.NewGuid()
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            // Now that we have ensured the tag exists, proceed to check for existing post tags
            if (await _context.PostTags.AnyAsync(pt => pt.BlogPostId == postId && pt.TagId == tag.Id))
            {
                return TagDTO.FromPostTag(tag);
            }

            // Create a new PostTag association
            var postTag = new PostTags
            {
                BlogPostId = postId,
                TagId = tag.Id
            };

            _context.PostTags.Add(postTag);
            await _context.SaveChangesAsync();

            return TagDTO.FromPostTag(tag);
        }

        public async Task<TagDTO> RemoveTagFromBlogPost(Guid postId, string tagName)
        {
            var postTag = await _context.PostTags
                .Include(pt => pt.Tag)
                .Where(pt => pt.BlogPostId == postId && pt.Tag.TagName == tagName)
                .FirstOrDefaultAsync();

            if (postTag != null)
            {
                _context.PostTags.Remove(postTag);
                await _context.SaveChangesAsync();
            }

            // Check if there are tags that are not associated with any blog post
            var tags = await _context.Tags
                .Where(t => t.PostTags.Count == 0)
                .ToListAsync();
            _context.Tags.RemoveRange(tags);
            
            return TagDTO.FromPostTag(postTag.Tag);
        }

        public async Task<TagDTO> GetTagByName(string name)
        {
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagName.Contains(name));
            return TagDTO.FromPostTag(tag);
        }
    }
}
