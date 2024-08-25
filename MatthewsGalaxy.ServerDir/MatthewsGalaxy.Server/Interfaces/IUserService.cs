using MatthewsGalaxy.Server.Data;
using Microsoft.AspNetCore.Identity;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
    }
}
