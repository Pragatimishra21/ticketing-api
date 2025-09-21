using TicketingSystem.Models;

namespace TicketingSystem.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(Users user);
        Users? GetUserById(int id);
        IEnumerable<Users> GetAllUsers();
    }
}
