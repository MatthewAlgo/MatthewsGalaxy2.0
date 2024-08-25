using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.DTOs;

namespace MatthewsGalaxy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _projectRepository.GetProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(string id)
        {
            try
            {
                var project = await _projectRepository.GetProjectById(id);
                return Ok(project);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with ID '{id}' not found.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] ProjectDTO project)
        {
            if (project == null)
            {
                return BadRequest("Project cannot be null.");
            }

            try
            {
                var createdProject = await _projectRepository.CreateProject(project);
                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> UpdateProject(string id, [FromBody] Project project)
        {
            if (project == null || id != project.Id)
            {
                return BadRequest("Project data is invalid.");
            }

            try
            {
                var updatedProject = await _projectRepository.UpdateProject(project);
                return Ok(updatedProject);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with ID '{id}' not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(string id)
        {
            try
            {
                var deletedProject = await _projectRepository.DeleteProject(id);
                return Ok(deletedProject);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with ID '{id}' not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
