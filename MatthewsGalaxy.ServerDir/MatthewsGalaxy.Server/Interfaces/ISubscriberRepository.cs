using MatthewsGalaxy.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface ISubscriberRepository
    {
        Task<IEnumerable<Subscriber>> GetSubscribersAsync();

        Task<Subscriber> GetByIdAsync(Guid id);

        Task<Subscriber> GetByEmailAsync(string email);

        Task<bool> IsSubscribedAsync(string email);

        Task AddSubscriberAsync(Subscriber subscriber);

        Task RemoveSubscriberAsync(string id);

        Task VerifySubscriberAsync(string id);

        Task UpdateAsync(Subscriber subscriber);

        Task SendEmailToAllSubscribersAsync(EmailRequest email);

        Task SendEmailToSubscriberAsync(EmailRequest email, string id);
    }
}