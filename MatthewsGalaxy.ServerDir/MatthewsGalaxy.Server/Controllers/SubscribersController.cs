using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MatthewsGalaxy.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly ILogger<SubscribersController> _logger;

        public SubscribersController(ISubscriberRepository subscriberRepository, ILogger<SubscribersController> logger)
        {
            _subscriberRepository = subscriberRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscriber>>> GetSubscribers()
        {
            try
            {
                var subscribers = await _subscriberRepository.GetSubscribersAsync();
                return Ok(subscribers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subscribers.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscriber>> GetSubscriberById(Guid id)
        {
            try
            {
                var subscriber = await _subscriberRepository.GetByIdAsync(id);

                if (subscriber == null)
                {
                    return NotFound();
                }

                return Ok(subscriber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching subscriber with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Subscriber>> GetSubscriberByEmail(string email)
        {
            try
            {
                var subscriber = await _subscriberRepository.GetByEmailAsync(email);

                if (subscriber == null)
                {
                    return NotFound();
                }

                return Ok(subscriber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching subscriber with email {email}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("issubscribed")]
        public async Task<ActionResult<bool>> IsSubscribed(string email)
        {
            try
            {
                var isSubscribed = await _subscriberRepository.IsSubscribedAsync(email);
                return Ok(isSubscribed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while checking subscription status for email {email}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddSubscriber([FromBody] Subscriber subscriber)
        {
            subscriber.Id = Guid.NewGuid();
            subscriber.Verified = false;
            try
            {
                await _subscriberRepository.AddSubscriberAsync(subscriber);
                return CreatedAtAction(nameof(GetSubscriberById), new { id = subscriber.Id }, subscriber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new subscriber.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveSubscriber(Guid id)
        {
            try
            {
                await _subscriberRepository.RemoveSubscriberAsync(id.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while removing subscriber with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("verify/{id}")]
        public async Task<ActionResult> VerifySubscriber(Guid id)
        {
            try
            {
                await _subscriberRepository.VerifySubscriberAsync(id.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while verifying subscriber with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSubscriber([FromBody] Subscriber subscriber)
        {
            try
            {
                await _subscriberRepository.UpdateAsync(subscriber);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating subscriber.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendEmailToAllSubscribers([FromBody] EmailRequest emailRequest)
        {
            try
            {
                await _subscriberRepository.SendEmailToAllSubscribersAsync(emailRequest);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email to all subscribers.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("send/{id}")]
        public async Task<ActionResult> SendEmailToSubscriber([FromBody] EmailRequest emailRequest, Guid id)
        {
            try
            {
                await _subscriberRepository.SendEmailToSubscriberAsync(emailRequest, id.ToString());
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while sending email to subscriber with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}