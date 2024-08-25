using System.ComponentModel.DataAnnotations;
using MatthewsGalaxy.Server.Data;

namespace MatthewsGalaxy.Server.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; }

        public UserDTO(User user)
        {
            if (user.Roles != null)
            {
                Roles = user.Roles;
            }
            UserName = user.UserName;
            Email = user.Email;
            EmailConfirmed = user.EmailConfirmed;
            Id = user.Id;
        }

        public UserDTO() { }

        public User ToUser()
        {
            return new User
            {
                UserName = UserName,
                Email = Email,
                EmailConfirmed = EmailConfirmed,
                Id = this.Id,
                Roles = Roles
            };
        }

        public User ToUser(User user)
        {
            user.UserName = UserName;
            user.Email = Email;
            user.EmailConfirmed = EmailConfirmed;
            user.Id = Id;
            user.Roles = Roles;
            return user;
        }

        public static UserDTO FromUser(User user)
        {
            return new UserDTO(user);
        }

        public static User FromUserDTO(UserDTO userDTO)
        {
            return userDTO.ToUser();
        }

    }
}
