using Dapper;
using System.Data;
using System.Data.Common;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;

namespace TicketingSystem.Repositories
{
    public class UserRepository(IDbConnection db) : IUserRepository
    {
        private readonly IDbConnection _db = db;

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            string sql = "SELECT * FROM Users WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<Users>(sql, new { Email = email });
        }

        public async Task<int> CreateUserAsync(Users user)
        {
            string sql = @"INSERT INTO Users (Name, Email, PasswordHash, Role, CreatedAt) 
                           VALUES (@Name, @Email, @PasswordHash, @Role, GETDATE());
                           SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _db.ExecuteScalarAsync<int>(sql, user);
        }

        public Users? GetUserById(int id)
        {
            var query = "SELECT Id, Name, Email, Role FROM Users WHERE Id = @Id";
            return _db.QueryFirstOrDefault<Users>(query, new { Id = id });
        }

        public IEnumerable<Users> GetAllUsers()
        {
            var query = "SELECT Id, Name, Email, Role FROM Users";
            return _db.Query<Users>(query);
        }

    }
}
