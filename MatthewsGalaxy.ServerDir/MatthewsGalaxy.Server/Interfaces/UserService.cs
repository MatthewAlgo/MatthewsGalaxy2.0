using System.Data;
using System.Security.Claims;
using MatthewsGalaxy.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Interfaces
{

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Name);
            var userEmail = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Email);

            if (userName is null || userEmail is null)
            {
                return null;
            }

            // Find the user by email and username
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.UserName == userName && u.Email == userEmail);

            if (user == null)
            {
                return null;
            }

            // Get the roles of the user
            var roles = await _userManager.GetRolesAsync(user);
            user.Roles = roles;

            return user;
        }

    }

}
