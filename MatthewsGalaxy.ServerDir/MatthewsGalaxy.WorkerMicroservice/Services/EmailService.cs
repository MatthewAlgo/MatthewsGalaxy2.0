using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatthewsGalaxy.WorkerMicroservice.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpClient _smtpClient;
        private IEmailRepository _emailRepository;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration, IEmailRepository emailRepository)
        {
            _logger = logger;
            _emailRepository = emailRepository;

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

        // Send verification email
        public async Task SendVerificationEmailAsync(string email, string verificationUrl)
        {
            var subject = "Verify your email address";
            var message = $"Please verify your email address by clicking <a href='{verificationUrl}'>here</a>. \n\nIf you did not request this, please ignore this email.";

            await _emailRepository.SendEmailAsync(email, subject, message);
        }

        // Send subscription verification email to user
        public async Task SendSubscriptionVerificationEmailAsync(string email, string verificationUrl, string removalUrl)
        {
            var subject = "Verify your email address for subscriptions";
            var message = $"Please verify your email address by clicking <a href='{verificationUrl}'>here</a>. " +
                          $"\n\nYou can unsubscribe at any time by clicking <a href='{removalUrl}'>here</a>." +
                          $"\n\nIf you did not request this, please ignore this email.";
            await _emailRepository.SendEmailAsync(email, subject, message);
        }
        
        public async Task SendNewPostEmailAsync(string email, string postTitle, string postUrl)
        {
            var subject = "New post published";
            var message = $"A new post titled '{postTitle}' has been published. You can read it <a href='{postUrl}'>here</a>.";

            await _emailRepository.SendEmailAsync(email, subject, message);
        }
    }
}