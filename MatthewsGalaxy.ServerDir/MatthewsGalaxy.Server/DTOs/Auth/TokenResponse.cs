namespace MatthewsGalaxy.Server.DTOs.Auth
{
    public class TokenResponse
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public string Token { get; set; }
    }
}
