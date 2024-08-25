using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.DTOs;

namespace MatthewsGalaxy.Server.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project> CreateProject(ProjectDTO project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            Project newProject = project.ToProject();
            newProject.Id = Guid.NewGuid().ToString();

            await _context.Projects.AddAsync(newProject);
            await _context.SaveChangesAsync();
            return newProject;
        }

        public async Task<Project> DeleteProject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Project ID cannot be null or empty", nameof(id));
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID '{id}' not found.");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> GetProjectById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Project ID cannot be null or empty", nameof(id));
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID '{id}' not found.");
            }

            return project;
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> UpdateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var existingProject = await _context.Projects.FindAsync(project.Id);
            if (existingProject == null)
            {
                throw new KeyNotFoundException($"Project with ID '{project.Id}' not found.");
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.ProjectType = project.ProjectType;
            existingProject.LocalImageLocation = project.LocalImageLocation;

            _context.Projects.Update(existingProject);
            await _context.SaveChangesAsync();

            return existingProject;
        }
    }
}
