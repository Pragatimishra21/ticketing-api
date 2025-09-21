namespace TicketingSystem.Models
{
    public class Tickets
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Status { get; set; } = "Open";      
        public string? Priority { get; set; } = "Medium";   
        public int? CreatedBy { get; set; }                
        public int? AssignedTo { get; set; }          
        public int? CategoryID { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
