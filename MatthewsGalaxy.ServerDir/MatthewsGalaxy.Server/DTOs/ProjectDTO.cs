using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.DTOs
{
    public class ProjectDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectType { get; set; }
        public string LocalImageLocation { get; set; }

        public string? ShortDescription { get; set; }
        // To project, from project

        public ProjectDTO(Project project)
        {
            Name = project.Name;
            Description = project.Description;
            ProjectType = project.ProjectType;
            LocalImageLocation = project.LocalImageLocation;
            ShortDescription = project.ShortDescription;
        }

        public ProjectDTO() { }

        public Project ToProject()
        {
            return new Project
            {
                Name = Name,
                Description = Description,
                ProjectType = ProjectType,
                LocalImageLocation = LocalImageLocation,
                ShortDescription = ShortDescription
            };
        }

        public Project ToProject(Project project)
        {
            project.Name = Name;
            project.Description = Description;
            project.ProjectType = ProjectType;
            project.LocalImageLocation = LocalImageLocation;
            project.ShortDescription = ShortDescription;
            return project;
        }

        public static ProjectDTO FromProject(Project project)
        {
            return new ProjectDTO(project);
        }

        public static Project FromProjectDTO(ProjectDTO projectDTO)
        {
            return projectDTO.ToProject();
        }

    }
}
