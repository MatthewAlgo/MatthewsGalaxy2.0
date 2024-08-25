using MatthewsGalaxy.Server.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MatthewsGalaxy.Server.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> Register(RegisterRequest request);
        Task<TokenResponse> Login(LoginRequest request);
        Task<bool> VerifyEmailAsync(string email, string token);
        JwtSecurityToken GetToken(IEnumerable<Claim> authClaims);
        string GetErrorsText(IEnumerable<IdentityError> errors);
    }
}
