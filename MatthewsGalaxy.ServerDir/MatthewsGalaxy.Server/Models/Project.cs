using System.ComponentModel.DataAnnotations;

namespace MatthewsGalaxy.Server.Models
{
    public class Project
    {
        // Each post can have multiple tags
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectType { get; set; }
        public string LocalImageLocation { get; set; }
        public string? ShortDescription { get; set; }

    }
}
