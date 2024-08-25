using MatthewsGalaxy.Server.Data;
using MatthewsGalaxy.Server.DTOs;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUser(Guid id);
        Task<User> GetIdentityUser(string username, string email); // Get the IdentityUser (User) object (not the DTO)
        Task<UserDTO> GetUser(string username, string email);
        Task<IEnumerable<UserDTO>> GetUsers(string username);
        Task<UserDTO?> CreateUser(UserDTO user);
        Task<UserDTO> UpdateUser(UserDTO user);
        Task<UserDTO> DeleteUser(string user);
        Task<string> GetRoleOfUserName(Guid id);
        Task<Boolean> IsUserAdmin(Guid id);
    }
}
