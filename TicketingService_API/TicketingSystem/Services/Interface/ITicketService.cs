using System.Net.Mail;
using TicketingSystem.Models;

namespace TicketingSystem.Services.Interface
{
    public interface ITicketService
    {
        Task<Tickets> CreateTicketAsync(Tickets ticket);
        Task<Tickets?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<Tickets>> GetTicketsByUserIdAsync(int userId);
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<bool> UpdateTicketStatusAsync(int ticketId, string status, int? assignedTo = null);
        Task<Attachments> AddAttachmentAsync(Attachments attachment);
        Task<IEnumerable<Attachments>> GetAttachmentsByTicketIdAsync(int ticketId);
        Task<bool> DeleteTicketsAsync(List<int> ticketIds);
    }
}
