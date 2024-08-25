using MatthewsGalaxy.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using MatthewsGalaxy.Server.DTOs.Auth;
using MatthewsGalaxy.Server.Interfaces;
using MatthewsGalaxy.Server.Models.Users;
using Microsoft.AspNetCore.WebUtilities;

namespace MatthewsGalaxy.Server.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor, ILogger<AuthenticationService> logger, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<TokenResponse> Register(RegisterRequest request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            var userByUsername = await _userManager.FindByNameAsync(request.Username);
            if (userByEmail is not null || userByUsername is not null)
            {
                throw new ArgumentException($"User with email {request.Email} or username {request.Username} already exists.");
            }

            // Every user is a guest user until they are assigned a role
            Guest user = new()
            {
                Email = request.Email,
                UserName = request.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ArgumentException($"Unable to register user {request.Username} errors: {GetErrorsText(result.Errors)}");
            }
            else
            {
                // Generate confirmation email token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                try
                {
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    await _emailService.SendVerificationEmailAsync(user.Email, encodedToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send verification email to {user.Email}");
                    await _userManager.DeleteAsync(user);
                    throw new Exception("Failed to send verification email. Please try again later.");
                }
            }

            if (!await _roleManager.RoleExistsAsync("Guest"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Guest"));
            }
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            await _userManager.AddToRoleAsync(user, "Guest");

            return new TokenResponse
            {
                Token = string.Empty,
                UserName = user.UserName,
                UserEmail = user.Email,
                UserRole = "Guest"
            };
        }


        public async Task<TokenResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username)
                       ?? await _userManager.FindByEmailAsync(request.Username);
            if (user is null)
            {
                throw new ArgumentException($"Unable to authenticate user {request.Username}. User not found");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new ArgumentException($"User {request.Username} has not confirmed their email address.");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new ArgumentException($"Unable to authenticate user {request.Username}");
            }

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Generate JWT token
            var token = GetToken(authClaims);

            // Get the user roles
            var roles = await _userManager.GetRolesAsync(user);

            user.Roles = roles.ToList();

            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = user.UserName,
                UserEmail = user.Email,
                UserRole = roles.FirstOrDefault()
            };
        }

        // Verify the email token
        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new ArgumentException($"User with email {email} not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        public string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}