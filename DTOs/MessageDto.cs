namespace VenderTest.DTOs
{
    // DTO for messages
    public class MessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
