using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using MatthewsGalaxy.WorkerMicroservice.Models;
using MatthewsGalaxy.WorkerMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using EmailRequest = MatthewsGalaxy.WorkerMicroservice.Models.EmailRequest;
using Subscriber = MatthewsGalaxy.Server.Models.Subscriber;

namespace MatthewsGalaxy.WorkerMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailService _emailService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public EmailController(IEmailRepository emailRepository, IEmailService emailService, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("send-verification-email/{destination}/{token}")]
        public async Task<IActionResult> SendVerificationEmailToUser(string destination, string token)
        {
            try
            {
                var verificationURL = $"{_configuration["Backend:BaseURL"]}/auth/User/verify-user-email?email={destination}&token={token}";
                await _emailService.SendVerificationEmailAsync(destination, verificationURL);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest email)
        {
            try
            {
                await _emailRepository.SendEmailAsync(email.DestinationEmail, email.Subject, email.Body);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add-email-address-subscriptions")]
        public async Task<IActionResult> AddEmailAddressSubscriptions([FromBody] string emailAddress)
        {
            try
            {
                Subscriber subscriber = new Subscriber
                {
                    Email = emailAddress,
                    Verified = false,
                    Id = Guid.NewGuid()
                };
                var response = await _emailRepository.AddEmailAddressSubscriptionsAsync(subscriber);

                if (response == null || response == false)
                {
                    return BadRequest("Failed to add email address. Most likely your email already exists");
                }

                // Send an email to the user to verify their email address
                var token = await _emailRepository.GetVerificationEmailTokenizedIdSubscriptionsAsync(emailAddress);
                var verificationURL = $"{_configuration["Backend:BaseURL"]}/api/EmailVerification/verify-email-address-subscriptions/{token}";
                var unsubscribeVerificationURL =
                    $"{_configuration["Backend:BaseURL"]}/api/EmailVerification/remove-email-address-subscriptions/{emailAddress}/{token}";
                await _emailService.SendSubscriptionVerificationEmailAsync(emailAddress, verificationURL,
                    unsubscribeVerificationURL);
                return Ok("Email address added successfully! Please verify your email address.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("verify-email-address-subscriptions/{token}")]
        public async Task<IActionResult> VerifyEmailAddressSubscriptions(string token)
        {
            try
            {
                if (await _emailRepository.VerifyEmailAddressSubscriptionsAsync(token))
                {

                    return Ok("Email address verified successfully!");
                }
                return BadRequest("Invalid token or verification failed.");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove-email-address-subscriptions")]
        public async Task<IActionResult> RemoveEmailAddressSubscriptions([FromBody] VerificationEmailRequest emailRequest)
        {
            try
            {
                await _emailRepository.RemoveEmailAddressSubscriptionsAsync(emailRequest);
                return Ok("Email address removed successfully! You will no longer receive emails from me :(.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove-email-address-subscriptions/{email}/{token}")]
        public async Task<IActionResult> RemoveEmailAddressSubscriptions(string email, string token)
        {
            try
            {
                await _emailRepository.RemoveEmailAddressSubscriptionsAsync(
                    new VerificationEmailRequest {Email = email, Token = token});
                return Ok("Email address removed successfully! You will no longer receive emails from me :(.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-verification-email-tokenized-id-subscriptions/{email}")]
        public async Task<IActionResult> GetVerificationEmailTokenizedIdSubscriptions(string email)
        {
            try
            {
                var token = await _emailRepository.GetVerificationEmailTokenizedIdSubscriptionsAsync(email);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("is-subscribed/{email}")]
        public async Task<IActionResult> IsSubscribed(string email)
        {
            try
            {
                var isSubscribed = await _emailRepository.EmailSubscriptionsExistsAsync(email);
                return Ok(isSubscribed);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
