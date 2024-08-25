using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public HomeController (IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet]
        [Route("error")]
        [ProducesResponseType(500, Type = typeof(string))] // Internal Server Error
        public IActionResult Error()
        {
            return StatusCode(500, "An error occurred");
        }

        [HttpGet]
        [Route("unauthorized")]

        [ProducesResponseType(401, Type = typeof(string))] // Unauthorized
        public IActionResult Unauthorized()
        {
            return StatusCode(401, "Unauthorized");
        }

        [HttpGet]
        [Route("forbidden")]
        [ProducesResponseType(403, Type = typeof(string))] // Forbidden
        public IActionResult Forbidden()
        {
            return StatusCode(403, "Forbidden");
        }
        [HttpGet]
        [Route("notfound")]
        [ProducesResponseType(404, Type = typeof(string))] // Not Found
        public IActionResult NotFound()
        {
            return StatusCode(404, "Not Found");
        }
        [HttpGet]
        [Route("badrequest")]
        [ProducesResponseType(400, Type = typeof(string))] // Bad Request
        public IActionResult BadRequest()
        {
            return StatusCode(400, "Bad Request");
        }
        [HttpGet]
        [Route("methodnotallowed")]
        [ProducesResponseType(405, Type = typeof(string))] // Method Not Allowed
        public IActionResult MethodNotAllowed()
        {
            return StatusCode(405, "Method Not Allowed");
        }

        [HttpGet]
        [Route("conflict")]
        [ProducesResponseType(409, Type = typeof(string))] // Conflict
        public IActionResult Conflict()
        {
            return StatusCode(409, "Conflict");
        }
        [HttpGet]
        [Route("unsupportedmediatype")]
        [ProducesResponseType(415, Type = typeof(string))] // Unsupported Media Type
        public IActionResult UnsupportedMediaType()
        {
            return StatusCode(415, "Unsupported Media Type");
        }

    }
}
