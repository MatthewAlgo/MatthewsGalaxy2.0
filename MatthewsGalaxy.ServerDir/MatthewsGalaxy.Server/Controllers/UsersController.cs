using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UsersController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        // Get the currently logged in user
        [HttpGet]
        [Authorize]
        [Route("current_user")]
        [ProducesResponseType(200, Type = typeof(User))] // OK
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (ModelState.IsValid)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))] // OK
        public async Task<IActionResult> GetUsers()
        {
            var loggedUser = await _userService.GetCurrentUserAsync();
            if (loggedUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }
            var users = await _userRepository.GetUsers();
            if (ModelState.IsValid)
            {
                return Ok(users);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("search/{id}")]
        [ProducesResponseType(200, Type = typeof(User))] // OK
        public async Task<IActionResult> GetUser(Guid id)
        {
            var loggedUser = await _userService.GetCurrentUserAsync();
            if (loggedUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }
            var user = await _userRepository.GetUser(id);
            if (ModelState.IsValid)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("search_uname/{username}")]
        [ProducesResponseType(200, Type = typeof(User))] // OK
        public async Task<IActionResult> GetUser(string username)
        {
            var loggedUser = await _userService.GetCurrentUserAsync();
            if (loggedUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }
            var users = await _userRepository.GetUsers(username);
            if (ModelState.IsValid)
            {
                return Ok(users);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        [ProducesResponseType(201, Type = typeof(User))] // Created
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var loggedUser = await _userService.GetCurrentUserAsync();
            if (loggedUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }
            UserDTO? newUser = await _userRepository.CreateUser(
                UserDTO.FromUser
                    (user));
            if (ModelState.IsValid && newUser != null)
            {
                return CreatedAtAction("GetUser", newUser);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        [ProducesResponseType(200, Type = typeof(User))] // OK
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var loggedUser = await _userService.GetCurrentUserAsync();
            if (loggedUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }
            var updatedUser = await _userRepository.UpdateUser(
                UserDTO.FromUser
                    (user));
            if (ModelState.IsValid)
            {
                return Ok(updatedUser);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{id}")]
        [ProducesResponseType(200, Type = typeof(User))] // OK
        public async Task<IActionResult> DeleteUser(string id)
        {
            var loggedUser = await _userService.GetCurrentUserAsync();
            if (loggedUser.Roles[0] != "Admin")
            {
                return Unauthorized();
            }

            // Get the user to delete
            var userToDelete = await _userRepository.GetUser(Guid.Parse(id));
            if (userToDelete == null)
            {
                return NotFound();
            }

            var user = await _userRepository.DeleteUser(id);
            if (ModelState.IsValid)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
