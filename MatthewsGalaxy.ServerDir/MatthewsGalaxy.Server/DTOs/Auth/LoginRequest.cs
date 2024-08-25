using Microsoft.Build.Framework;

namespace MatthewsGalaxy.Server.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
