using System.Net.Mail;
using TicketingSystem.Models;

namespace TicketingSystem.Repositories.Interface
{
    public interface ITicketRepository
    {
        Task<int> CreateTicketAsync(Tickets ticket);
        Task<Tickets?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<Tickets>> GetTicketsByUserIdAsync(int userId);
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<int> UpdateTicketStatusAsync(int ticketId, string status, int? assignedTo = null);
        Task<int> AddAttachmentAsync(Attachments attachment);
        Task<IEnumerable<Attachment>> GetAttachmentsByTicketIdAsync(int ticketId);
        Task<bool> DeleteTicketsAsync(List<int> ticketIds);
    }
}
