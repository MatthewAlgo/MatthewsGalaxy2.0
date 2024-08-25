using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;
using MatthewsGalaxy.Server.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MatthewsGalaxy.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            // Initialize users as a List<UserDTO> for dynamic collection
            List<UserDTO> users = new List<UserDTO>();

            // Retrieve all users from the context
            var allUsers = await _context.Users.ToListAsync();

            // Process each user to get roles and convert to UserDTO
            foreach (var user in allUsers)
            {
                if (user != null)
                {
                    // Get the roles of the user
                    var roles = await _userManager.GetRolesAsync(user);
                    user.Roles = roles;

                    // Add the UserDTO to the list
                    users.Add(UserDTO.FromUser(user));
                }
            }

            // Return the list of UserDTOs
            return users;
        }



        public async Task<UserDTO> GetUser(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.Id == id.ToString()).ContinueWith(t => UserDTO.FromUser(t.Result));
        }

        public async Task<User> GetIdentityUser(string username, string email)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.UserName == username && u.Email == email);
        }   

        public async Task<IEnumerable<UserDTO>> GetUsers(string username)
        {
            // Returns all users that contain the given username
            return await _context.Users
                .Where(u => u.UserName.Contains(username))
                .Select(u => UserDTO.FromUser(u)).ToListAsync();
        }

        // Get user by username and email
        public async Task<UserDTO> GetUser(string username, string email)
        {

            var user = await _context.Users
                .Where(u => u.UserName == username && u.Email == email)
                .Select(u => new UserDTO
                {
                    UserName = u.UserName,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserDTO?> CreateUser(UserDTO user)
        {
            // Only create the user if it doesn't already exist and the email is unique
            if (_context.Users.Any(u => u.UserName == user.UserName) || _context.Users.Any(u => u.Email == user.Email))
            {
                return null;
            }
            _context.Users.Add(user.ToUser());
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            _context.Users.Update(user.ToUser());
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserDTO> DeleteUser(string id)
        {
            try
            {
                // Check if the entity is already being tracked
                var existingUser = _context.Users.Local.FirstOrDefault(u => u.Id == id);
                if (existingUser != null)
                {
                    _context.Entry(existingUser).State = EntityState.Detached;
                }

                var userToDelete = await _context.Users.FirstOrDefaultAsync(
                    u => u.Id == id);
                if (userToDelete != null) {
                    _context.Users.Remove(userToDelete);
                    await _context.SaveChangesAsync();
                }
                return UserDTO.FromUser(userToDelete);
            }
            catch (Exception e)
            {
                // Handle exception
                Console.WriteLine(e.Message);
            }
            return null;
            
        }

        public Task<string> GetRoleOfUserName(Guid id)
        {
            return Task.FromResult(_context.UserRoles.Find(id).RoleId);
        }

        public async Task<Boolean> IsUserAdmin(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Id == id.ToString());
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
