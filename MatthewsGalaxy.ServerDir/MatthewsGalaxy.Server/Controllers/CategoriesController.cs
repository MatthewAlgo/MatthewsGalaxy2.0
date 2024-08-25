using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;

        public CategoriesController(ICategoryRepository categoryRepository, IUserService userService)
        {
            _categoryRepository = categoryRepository;
            _userService = userService;
        }  

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))] // OK
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            if (ModelState.IsValid)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet]
        [Route("search/{id}")]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryRepository.GetCategory(id);
            if (ModelState.IsValid)
            {
                return Ok(category);
            }
            else
            {  
                return BadRequest(ModelState);
            }
        }


        [HttpGet]
        [Route("search_urlhandle/{urlHandle}")]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> GetCategory(string urlHandle)
        {
            var category = await _categoryRepository.GetCategory(urlHandle);
            if (ModelState.IsValid)
            {
                return Ok(category);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
            // Check if the logged in user is an admin
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }


            var newCategory = await _categoryRepository.CreateCategory(
                    category
                );
            if (ModelState.IsValid)
            {
                return Ok(newCategory);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> UpdateCategory(CategoryDTO category)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            var updatedCategory = await _categoryRepository.UpdateCategory(
                    category.ToCategory()
                );
            if (ModelState.IsValid)
            {
                return Ok(updatedCategory);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(200, Type = typeof(Category))] // OK
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }


            var deletedCategory = await _categoryRepository.DeleteCategory(id);
            if (ModelState.IsValid)
            {
                return Ok(deletedCategory);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
