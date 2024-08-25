using MatthewsGalaxy.WorkerMicroservice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;

namespace MatthewsGalaxy.WorkerMicroservice.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpClient _smtpClient;
        private readonly IUnitOfWork _unitOfWork;

        public EmailRepository(ILogger<EmailService> logger, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            // Retrieve SMTP settings from configuration
            var smtpSection = configuration.GetSection("Email");
            var smtpServer = smtpSection.GetValue<string>("SmtpServer");
            var smtpPort = smtpSection.GetValue<int>("SmtpPort");
            var smtpUsername = smtpSection.GetValue<string>("SmtpUsername");
            var smtpPassword = smtpSection.GetValue<string>("SmtpPassword");

            // Configure SMTP settings
            _smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("matthewtest0000@gmail.com"), // Use a valid email address here
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                _logger.LogInformation($"Sending email to {email} with subject {subject}.");

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email.");
            }
        }

        public async Task<Boolean> VerifyEmailAddressSubscriptionsAsync(string token)
        {
            try
            {
                var Id = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                if (!Guid.TryParse(Id, out Guid parsedId))
                {
                    _logger.LogWarning($"Invalid GUID format for Id {Id}.");
                    return false;
                }

                var subscriber = await _unitOfWork.Subscribers
                    .GetAllAsync()
                    .ContinueWith(
                        task => task.Result.FirstOrDefault(s => s.Id == parsedId));

                if (subscriber == null)
                {
                    _logger.LogWarning($"Subscriber with Id {Id} not found.");
                    return false;
                }

                subscriber.Verified = true;

                await _unitOfWork.Subscribers.UpdateAsync(subscriber);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Subscriber with Id {Id} verified.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying email address.");
                return false;
            }
        }


        public async Task<Boolean> AddEmailAddressSubscriptionsAsync(Subscriber subscriber)
        {
            try
            {
                // If the email address is already subscribed, return
                if (await EmailSubscriptionsExistsAsync(subscriber.Email))
                {
                    _logger.LogWarning($"Subscriber with email {subscriber.Email} already exists.");
                    return false;
                }
                await _unitOfWork.Subscribers.AddAsync(subscriber);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Subscriber with email {subscriber.Email} added.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding email address subscriptions.");
                return false;
            }
        }

        public async Task RemoveEmailAddressSubscriptionsAsync(VerificationEmailRequest emailRequest)
        {
            try
            {
                var subscriber = await _unitOfWork.Subscribers
                    .GetAllAsync()
                    .ContinueWith(
                        task => task.Result.FirstOrDefault(s => s.Email == emailRequest.Email));

                // Check if the tokenized Id matches the email address
                if (subscriber == null || subscriber.Id != Guid.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(emailRequest.Token))))
                {
                    _logger.LogWarning($"Subscriber with email {emailRequest.Email} not found.");
                    return;
                }

                await _unitOfWork.Subscribers.DeleteAsync(subscriber.Id);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Subscriber with email {emailRequest.Email} removed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing email address subscriptions.");
            }
        }

        public async Task<string> GetVerificationEmailTokenizedIdSubscriptionsAsync(string email)
        {
            try
            {
                var subscriber = await _unitOfWork.Subscribers
                    .GetAllAsync()
                    .ContinueWith(
                        task => task.Result.FirstOrDefault(s => s.Email == email));

                if (subscriber == null)
                {
                    _logger.LogWarning($"Subscriber with email {email} not found.");
                    return null;
                }

                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(subscriber.Id.ToString()));

                _logger.LogInformation($"Token for subscriber with email {email} generated.");

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token for email address.");
                return null;
            }
        }

        // Check if email exists
        public async Task<bool> EmailSubscriptionsExistsAsync(string email)
        {
            var subscriber = await _unitOfWork.Subscribers
                .GetAllAsync()
                .ContinueWith(
                    task => task.Result.FirstOrDefault(s => s.Email == email));

            return subscriber != null;
        }


    }
}
