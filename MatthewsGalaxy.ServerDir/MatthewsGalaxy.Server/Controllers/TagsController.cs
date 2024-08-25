using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))] // OK
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagRepository.GetTags();
            if (ModelState.IsValid)
            {
                return Ok(tags);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("search/{id}")]
        [ProducesResponseType(200, Type = typeof(Tag))] // OK
        public async Task<IActionResult> GetTag(Guid id)
        {
            var tag = await _tagRepository.GetTag(id);
            if (ModelState.IsValid)
            {
                return Ok(tag);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get tags by blog post
        [HttpGet("post/{postId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))] // OK
        public async Task<IActionResult> GetTagsByBlogPost(Guid postId)
        {
            var tags = await _tagRepository.GetTagsByBlogPost(postId);
            if (ModelState.IsValid)
            {
                return Ok(tags);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Get tags by tag name 
        [HttpGet("search_tagname/{tagName}")]
        [ProducesResponseType(200, Type = typeof(Tag))] // OK
        public async Task<IActionResult> GetTag(string tagName)
        {
            var tags = await _tagRepository.GetTagsByTagName(tagName);
            if (ModelState.IsValid)
            {
                return Ok(tags);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201, Type = typeof(Tag))] // Created
        public async Task<IActionResult> CreateTag([FromBody] Tag tag)
        {
            await _tagRepository.CreateTag(tag);
            if (ModelState.IsValid)
            {
                return Created("tag", tag);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Authorize (Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(Tag))] // OK
        public async Task<IActionResult> UpdateTag([FromBody] Tag tag)
        {
            var updatedTag = await _tagRepository.UpdateTag(tag);
            if (ModelState.IsValid)
            {
                return Ok(updatedTag);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Authorize (Roles = "Admin")]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Tag))] // OK
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await _tagRepository.DeleteTag(id);
            if (ModelState.IsValid)
            {
                return Ok(tag);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
