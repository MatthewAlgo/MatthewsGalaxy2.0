using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using MatthewsGalaxy.WorkerMicroservice.Models;
using MatthewsGalaxy.WorkerMicroservice.Repository;
using MatthewsGalaxy.WorkerMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailRequest = MatthewsGalaxy.WorkerMicroservice.Models.EmailRequest;

namespace MatthewsGalaxy.WorkerMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApiService _apiService;
        private readonly IEmailService _emailService;
        private readonly IEmailRepository _emailRepository;

        public SubscribersController(
            IUnitOfWork unitOfWork,
            IApiService apiService, IEmailService emailService, IEmailRepository emailRepository)
        {
            _unitOfWork = unitOfWork;
            _apiService = apiService;
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        [HttpGet] // In the microservice
        public async Task<IActionResult> GetSubscribers()
        {
            var subscribers = await _unitOfWork.Subscribers.GetAllAsync();
            return Ok(subscribers);
        }

        // Get by ID
        [HttpGet("{id}")] // In the microservice
        public async Task<IActionResult> GetSubscriberById(Guid id)
        {
            var subscriber = await _unitOfWork.Subscribers.GetByIdAsync(id);

            if (subscriber == null)
            {
                return NotFound();
            }

            return Ok(subscriber);
        }

        // Get subscribers by email
        [HttpGet("email/{email}")] // In the microservice
        public async Task<IActionResult> GetSubscriberByEmail(string email)
        {
            var subscriber = await _unitOfWork.Subscribers
                .GetAllAsync()
                .ContinueWith(
                    task => task.Result.FirstOrDefault(s => s.Email == email)); 

            if (subscriber == null)
            {
                return NotFound();
            }

            return Ok(subscriber);
        }

        [HttpPost] // In the microservice
        public async Task<IActionResult> AddSubscriber([FromBody] Server.Models.Subscriber subscriber) 
        {
            if (subscriber == null)
            {
                return BadRequest();
            }

            // Check if the email is already subscribed
            var existingSubscriber = await _unitOfWork.Subscribers
                            .GetAllAsync()
                            .ContinueWith(
                                task => task.Result.FirstOrDefault(s => s.Email == subscriber.Email));

            if (existingSubscriber != null) {   
                return BadRequest("The email is already subscribed.");
            }

            await _unitOfWork.Subscribers.AddAsync(subscriber);
            await _unitOfWork.SaveAsync();

            return Ok(subscriber);
        }

        [HttpDelete("{id}")] // In the microservice
        public async Task<IActionResult> DeleteSubscriber(string id)
        {
            var subscriber = await _unitOfWork.Subscribers.GetByIdAsync(Guid.Parse(id));

            if (subscriber == null)
            {
                return NotFound();
            }

            _unitOfWork.Subscribers.Delete(subscriber);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPost("send")] // In the microservice
        public async Task<IActionResult> SendEmailToSubscribers([FromBody] EmailRequest request)
        {
            var subscribers = await _unitOfWork.Subscribers.GetAllAsync();

            foreach (var subscriber in subscribers)
            {
                await _emailRepository.SendEmailAsync(subscriber.Email, request.Subject, request.Body);
            }

            return Ok();
        }

        [HttpPost("send/{id}")] // In the microservice
        public async Task<IActionResult> SendEmailToSubscriber(string id, [FromBody] EmailRequest request)
        {
            var subscriber = await _unitOfWork.Subscribers.GetByIdAsync(Guid.Parse(id));

            if (subscriber == null)
            {
                return NotFound();
            }

            await _emailRepository.SendEmailAsync(subscriber.Email, request.Subject, request.Body);

            return Ok();
        }

        // Update a subscriber
        [HttpPut] // In the microservice
        public async Task<IActionResult> UpdateSubscriber([FromBody] Server.Models.Subscriber subscriber)
        {
            var existingSubscriber = await _unitOfWork.Subscribers.GetByIdAsync(subscriber.Id);

            if (existingSubscriber == null)
            {
                return NotFound();
            }

            existingSubscriber.Email = subscriber.Email;
            existingSubscriber.Id = subscriber.Id;
            existingSubscriber.Verified = subscriber.Verified;

            _unitOfWork.Subscribers.Update(existingSubscriber);
            await _unitOfWork.SaveAsync();

            return Ok(existingSubscriber);
        }
    }
}
