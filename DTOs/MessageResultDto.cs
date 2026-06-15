namespace VenderTest.DTOs
{
    public class MessageResultDto
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string MessageText { get; set; }

        public string MessageType { get; set; }

        public DateTime SentAt { get; set; }

        public string Status { get; set; }
           public int IsSent { get; set; }       
    public int IsDelivered { get; set; }  
    public int IsSeen { get; set; }
    }
}
