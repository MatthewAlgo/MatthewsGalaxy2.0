using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.WorkerMicroservice.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationUrl);
        Task SendNewPostEmailAsync(string email, string postTitle, string postUrl);
        Task SendSubscriptionVerificationEmailAsync(string email, string verificationUrl, string removalUrl);
    }
}
