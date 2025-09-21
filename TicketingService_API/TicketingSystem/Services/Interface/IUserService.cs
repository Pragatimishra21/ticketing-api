using Microsoft.AspNetCore.Identity.Data;
using TicketingSystem.Models;

namespace TicketingSystem.Services.Interface
{
    public interface IUserService
    {
        Task<LoginResponse?> LoginAsync(UserLoginRequest request);
        Task<LoginResponse> RegisterAsync(UserRegisterRequest request);
        Users GetUserById(int id);
        IEnumerable<Users> GetAllUsers();
    }
}
