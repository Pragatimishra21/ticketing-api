using Dapper;
using System.Data;
using System.Net.Mail;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;

namespace TicketingSystem.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IDbConnection _db;

        public TicketRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateTicketAsync(Tickets ticket)
        {
            string sql = @"
                INSERT INTO Tickets 
                (Title, Description, Status, Priority, CreatedBy, AssignedTo, CategoryID, CreatedAt, UpdatedAt)
                VALUES 
                (@Title, @Description, @Status, @Priority, @CreatedBy, @AssignedTo, @CategoryID, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            return await _db.ExecuteScalarAsync<int>(sql, ticket);
        }

        public async Task<Tickets?> GetTicketByIdAsync(int ticketId)
        {
            string sql = "SELECT * FROM Tickets WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Tickets>(sql, new { Id = ticketId });
        }

        public async Task<IEnumerable<Tickets>> GetTicketsByUserIdAsync(int userId)
        {
            string sql = "SELECT * FROM Tickets WHERE CreatedBy = @UserId";
            return await _db.QueryAsync<Tickets>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            string sql = @"
        SELECT 
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.Priority,
            t.CreatedAt,
            COALESCE(u1.Name, '') AS CreatedByName,
            COALESCE(u2.Name, '') AS AssignedToName
        FROM Tickets t
        LEFT JOIN Users u1 ON t.CreatedBy = u1.Id
        LEFT JOIN Users u2 ON t.AssignedTo = u2.Id
        ORDER BY t.CreatedAt DESC";

            return await _db.QueryAsync<TicketDto>(sql);
        }

        public async Task<int> UpdateTicketStatusAsync(int ticketId, string status, int? assignedTo = null)
        {
            string sql = @"
                UPDATE Tickets
                SET Status = @Status, AssignedTo = @AssignedTo, UpdatedAt = GETDATE()
                WHERE Id = @Id";

            return await _db.ExecuteAsync(sql, new { Id = ticketId, Status = status, AssignedTo = assignedTo });
        }

        public async Task<int> AddAttachmentAsync(Attachments attachment)
        {
            string sql = @"
                INSERT INTO Attachments 
                (TicketId, UploadedBy, FilePath, CreatedAt)
                VALUES 
                (@TicketId, @UploadedBy, @FilePath, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            return await _db.ExecuteScalarAsync<int>(sql, attachment);
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByTicketIdAsync(int ticketId)
        {
            string sql = "SELECT * FROM Attachments WHERE TicketId = @TicketId";
            return await _db.QueryAsync<Attachment>(sql, new { TicketId = ticketId });
        }

        public async Task<bool> DeleteTicketsAsync(List<int> ticketIds)
        {
            string sql = "DELETE FROM Tickets WHERE Id IN @Ids";
            var rowsAffected = await _db.ExecuteAsync(sql, new { Ids = ticketIds });
            return rowsAffected > 0;
        }

    }
}

