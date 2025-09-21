using System.Net.Mail;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository repository)
        {
            _ticketRepository = repository;
        }

        public async Task<Tickets> CreateTicketAsync(Tickets ticket)
        {
            ticket.Id = await _ticketRepository.CreateTicketAsync(ticket);
            return ticket;
        }

        public async Task<Tickets?> GetTicketByIdAsync(int ticketId)
        {
            return await _ticketRepository.GetTicketByIdAsync(ticketId);
        }

        public async Task<IEnumerable<Tickets>> GetTicketsByUserIdAsync(int userId)
        {
            return await _ticketRepository.GetTicketsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllTicketsAsync();
        }

        public async Task<bool> UpdateTicketStatusAsync(int ticketId, string status, int? assignedTo = null)
        {
            var rows = await _ticketRepository.UpdateTicketStatusAsync(ticketId, status, assignedTo);
            return rows > 0;
        }

        public async Task<Attachments> AddAttachmentAsync(Attachments attachment)
        {
            attachment.Id = await _ticketRepository.AddAttachmentAsync(attachment);
            return attachment;
        }

        public async Task<IEnumerable<Attachments>> GetAttachmentsByTicketIdAsync(int ticketId)
        {
            return (IEnumerable<Attachments>)await _ticketRepository.GetAttachmentsByTicketIdAsync(ticketId);
        }

        public async Task<bool> DeleteTicketsAsync(List<int> ticketIds)
        {
            return await _ticketRepository.DeleteTicketsAsync(ticketIds);
        }

    }
}
