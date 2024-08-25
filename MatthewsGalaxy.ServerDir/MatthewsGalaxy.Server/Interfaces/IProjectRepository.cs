using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectById(string id);

        // Create a project
        Task<Project> CreateProject(ProjectDTO project);
        Task<Project> UpdateProject(Project project);
        Task<Project> DeleteProject(string id);
    }
}
