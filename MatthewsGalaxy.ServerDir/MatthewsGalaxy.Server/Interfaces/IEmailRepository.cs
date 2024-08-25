using MatthewsGalaxy.Server.Models;
using System.Net.Http;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IEmailRepository
    {
        Task<string> GetEmailVerificationTokenizedId(string destination);
        Task<string> AddEmailAddress(string emailAddress);
        Task<string> VerifyEmail(string token);
        Task<string> RemoveEmailAddress(VerificationEmailRequest request);
        Task<Boolean> RemoveEmailAddressByEmail(string email, string token);
    }
}
