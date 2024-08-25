using System.Threading.Tasks; // Make sure to include this for Task
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController] // Optional but recommended for automatic model validation and route prefixing
    [Route("api/[controller]")] // Defines the route template for the controller
    public class EmailVerificationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailService _emailService;

        public EmailVerificationController(
            IAuthenticationService authenticationService,
            IEmailRepository emailRepository,
            IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        [HttpGet("verify-email-address-subscriptions/{token}")]
        [ProducesResponseType(200, Type = typeof(string))] // OK
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> VerifyEmailAddressSubscriptions(string token)
        {
            var response = await _emailRepository.VerifyEmail(token);
            if (response != null) // Check response instead of ModelState for custom validation logic
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Invalid token or verification failed.");
            }
        }

        [HttpPost("add-email-address-subscriptions/{email}")]
        [ProducesResponseType(200, Type = typeof(string))] // OK
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> AddEmailAddress(string email)
        {
            if (!IsValidEmail(email)) // Custom validation logic for email
            {
                return BadRequest("Invalid email address.");
            }

            try
            {
                var response = await _emailRepository.AddEmailAddress(email);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Failed to add email address.");
                }
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("remove-email-address-subscriptions/{email}/{token}")]
        [ProducesResponseType(200, Type = typeof(string))] // OK
        [ProducesResponseType(400)] // Bad Request
        public async Task<IActionResult> RemoveEmailAddressSubscriptions(string email, string token)
        {
            if (!IsValidEmail(email)) // Custom validation logic for email
            {
                return BadRequest("Invalid email address.");
            }

            var response = await _emailRepository.RemoveEmailAddressByEmail(email, token);
            if (response != null)
            {
                return Ok("Email removed successfully. I am sorry to see you go :(");
            }
            else
            {
                return BadRequest("Failed to remove email address or invalid token.");
            }
        }

        private bool IsValidEmail(string email)
        {
            // A simple email validation check
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }
    }
}
