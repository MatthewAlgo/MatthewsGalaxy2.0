using System.Text;
using MatthewsGalaxy.Server.Configuration;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MatthewsGalaxy.Server.Service
{
    // Commmunicates with the email microservice to send emails
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public EmailService(HttpClient httpClient, IOptions<EmailSettings> emailSettings)
        {
            _httpClient = httpClient;
            _apiBaseUrl = emailSettings.Value.MicroserviceAPI; // Get the API URL from configuration
        }

        public async Task SendEmailAsync(EmailRequest email)
        {
            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Email/send-email", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to send email");
            }
        }

        // Send a verification email to a user
        public async Task SendVerificationEmailAsync(string destination, string verificationURL)
        {
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Email/send-verification-email/{destination}/{verificationURL}"
                , null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to send verification email");
            }
        }
    }
}
