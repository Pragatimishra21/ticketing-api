using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using TicketingSystem.Models;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService service)
        {
            _ticketService = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Tickets ticket)
        {
            var created = await _ticketService.CreateTicketAsync(ticket);
            return Ok(created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserTickets(int userId)
        {
            var tickets = await _ticketService.GetTicketsByUserIdAsync(userId);
            return Ok(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status, [FromQuery] int? assignedTo = null)
        {
            var updated = await _ticketService.UpdateTicketStatusAsync(id, status, assignedTo);
            if (!updated) return BadRequest();
            return Ok(new { Message = "Ticket updated successfully" });
        }

        [HttpPost("{id}/attachments")]
        public async Task<IActionResult> AddAttachment(int id, [FromBody] Attachments attachment)
        {
            attachment.TicketId = id;
            var added = await _ticketService.AddAttachmentAsync(attachment);
            return Ok(added);
        }

        [HttpGet("{id}/attachments")]
        public async Task<IActionResult> GetAttachments(int id)
        {
            var attachments = await _ticketService.GetAttachmentsByTicketIdAsync(id);
            return Ok(attachments);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTickets([FromBody] List<int> ticketIds)
        {
            var result = await _ticketService.DeleteTicketsAsync(ticketIds);

            if (result)
                return Ok(new { message = "Tickets deleted successfully" });
            return StatusCode(500, "Error deleting tickets");
        }

    }
}
