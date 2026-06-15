namespace VenderTest.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }

        // Receiver of the notification (logged-in user)
        public int ReceiverId { get; set; }

        // Sender of the message
        public int SenderId { get; set; }

        public int MessageId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        // Optional: include message info if you want to return it in the DTO
        public string? MessageText { get; set; }
        public string? MessageType { get; set; }
        public DateTime? SentAt { get; set; }
    }
}