using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class TagDTO
    {
        public BlogPostDTO BlogPost { get; set; }
        public string TagName { get; set; }
        public string? TagDescription { get; set; }
        public string? TagIconURL { get; set; }

        public TagDTO(Tag postTag)
        {
            TagName = postTag.TagName;
            TagDescription = postTag.TagDescription;
            TagIconURL = postTag.TagIconURL;
        }

        public TagDTO() { }

        public Tag ToPostTag()
        {
            return new Tag
            {
                TagName = TagName,
                TagDescription = TagDescription,
                TagIconURL = TagIconURL
            };
        }

        public Tag ToPostTag(Tag postTag)
        {
            postTag.TagName = TagName;
            postTag.TagDescription = TagDescription;
            postTag.TagIconURL = TagIconURL;
            return postTag;
        }

        public static TagDTO FromPostTag(Tag postTag)
        {
            return new TagDTO(postTag);
        }

        public static Tag FromPostTagDTO(TagDTO postTagDTO)
        {
            return postTagDTO.ToPostTag();
        }

    }
}
