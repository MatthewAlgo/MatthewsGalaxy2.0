using MatthewsGalaxy.Server.Configuration;
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using MatthewsGalaxy.Server.Models;
using System.Text;
using NuGet.Common;

namespace MatthewsGalaxy.Server.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        public EmailRepository(HttpClient httpClient, IOptions<EmailSettings> emailSettings)
        {
            _httpClient = httpClient;
            _apiBaseUrl = emailSettings.Value.MicroserviceAPI; // Get the API URL from configuration
        }

        public async Task<string> GetEmailVerificationTokenizedId(string destination)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Email/get-verification-email-tokenized-id-subscriptions/{destination}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get email verification tokenized id");
            }
            return await response.Content.ReadAsStringAsync();
        }

        // Add email address
        public async Task<string> AddEmailAddress(string emailAddress)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Email/add-email-address-subscriptions", emailAddress);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to add email address. Most likely your email already exists");
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> VerifyEmail(string token)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Email/verify-email-address-subscriptions/{token}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to verify email");
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RemoveEmailAddress(VerificationEmailRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Email/remove-email-address-subscriptions", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to remove email address");
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Boolean> RemoveEmailAddressByEmail(string email, string token) {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Email/remove-email-address-subscriptions/{email}/{token}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to remove email address");
            }
            return true;
        }
    }
}
