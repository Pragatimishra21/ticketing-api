using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TicketingSystem.Exceptions;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(UserLoginRequest request)
        {
            if (request.Email == "admin@admin.com" && request.Password == "Admin@123")
            {
                var adminUser = new Users
                {
                    Name = "Admin User",
                    Email = "admin@admin.com",
                    Role = "Admin"
                };
                return GenerateJwtToken(adminUser);
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new AuthException("No account found with this email.");

            if (!VerifyPassword(request.Password, user.PasswordHash))
                throw new AuthException("Invalid password.");

            return GenerateJwtToken(user);
        }

        public async Task<LoginResponse> RegisterAsync(UserRegisterRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new AuthException("Email is already registered.");

            var role = request.Email.EndsWith("@admin.com") ? "Admin" : "Client";
            var user = new Users
            {
                Name = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = role
            };

            int newUserId = await _userRepository.CreateUserAsync(user);
            user.Id = newUserId;
            if (newUserId <= 0)
                throw new AuthException("Failed to register user.");

            return GenerateJwtToken(user);
        }

        public Users GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            return user;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return users;
        }


        private LoginResponse GenerateJwtToken(Users user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
