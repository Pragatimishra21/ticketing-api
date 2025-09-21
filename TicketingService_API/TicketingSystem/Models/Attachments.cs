namespace TicketingSystem.Models
{
    public class Attachments
    {
        public int Id { get; set; }
        public int? TicketId { get; set; }
        public int? UploadedBy { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
