using MatthewsGalaxy.Server.Models;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest email);

        Task SendVerificationEmailAsync(string destination, string verificationURL);

    }
}
