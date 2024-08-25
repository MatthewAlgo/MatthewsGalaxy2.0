using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.WorkerMicroservice.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(string destinationEmail, string subject, string body);

        Task<Boolean> AddEmailAddressSubscriptionsAsync(Subscriber subscriber);

        Task RemoveEmailAddressSubscriptionsAsync(VerificationEmailRequest emailRequest);

        Task<string> GetVerificationEmailTokenizedIdSubscriptionsAsync(string email);
        Task<Boolean> VerifyEmailAddressSubscriptionsAsync(string token);
        Task<bool> EmailSubscriptionsExistsAsync(string email);
    }
}
