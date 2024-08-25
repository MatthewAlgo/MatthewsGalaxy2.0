using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using MatthewsGalaxy.Server.Configuration;
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace MatthewsGalaxy.Server.Repository
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public SubscriberRepository(HttpClient httpClient, IOptions<EmailSettings> emailSettings)
        {
            _httpClient = httpClient;
            _apiBaseUrl = emailSettings.Value.MicroserviceAPI; // Get the API URL from configuration
        }

        public async Task<IEnumerable<Subscriber>> GetSubscribersAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Subscribers");
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to retrieve subscribers from the microservice. - Reason:" + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var subscribers = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Subscriber>>(content);
                return subscribers;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
            return null;
        }

        public async Task<Subscriber> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Subscribers/{id}");
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to retrieve the subscriber from the microservice. - Reason:" + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var subscriber = Newtonsoft.Json.JsonConvert.DeserializeObject<Subscriber>(content);
                return subscriber;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
            return null;
        }

        public async Task<Subscriber> GetByEmailAsync(string email) // Email address
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Subscribers/email/{email}");
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to retrieve the subscriber from the microservice. - Reason:" + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var subscriber = Newtonsoft.Json.JsonConvert.DeserializeObject<Subscriber>(content);
                return subscriber;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> IsSubscribedAsync(string email)
        {
            var subscriber = await GetByEmailAsync(email);
            if (subscriber != null && subscriber.Verified)
            {
                return true;
            }
            return false;
        }

        public async Task AddSubscriberAsync(Subscriber subscriber)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(subscriber), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Subscribers", jsonContent);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to add the subscriber to the microservice. - Reason:" + response.ReasonPhrase);
            }
        }

        public async Task RemoveSubscriberAsync(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_apiBaseUrl}/Subscribers/{id}");
            var response = await _httpClient.SendAsync(request);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to remove the subscriber from the microservice. - Reason:" + response.ReasonPhrase);
            }
        }

        public async Task VerifySubscriberAsync(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiBaseUrl}/Subscribers/verify/{id}");
            var response = await _httpClient.SendAsync(request);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to verify the subscriber in the microservice. - Reason:" + response.ReasonPhrase);
            }
        }

        public async Task UpdateAsync(Subscriber subscriber)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(subscriber), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/Subscribers", jsonContent);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to update the subscriber in the microservice. - Reason:" + response.ReasonPhrase);
            }
        }

        // Send email to subscribers
        public async Task SendEmailToAllSubscribersAsync(EmailRequest email)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(email), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Subscribers/send", jsonContent);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to send email to subscribers. - Reason:" + response.ReasonPhrase);
            }
        }

        // Send email to a single subscriber
        public async Task SendEmailToSubscriberAsync(EmailRequest email, string id)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(email), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Subscribers/send/{id}", jsonContent);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException) {
                throw new Exception("Failed to send email to the subscriber. - Reason:" + response.ReasonPhrase);
            }
        }

    }
}
