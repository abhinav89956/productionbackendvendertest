namespace VenderTest.DTOs
{
    public class ContactDto
    {
        public int UserId { get; set; }

        public int ContactUserId { get; set; }
        public string VenderCode { get; set; }

        public string Email { get; set; }

        public DateTime? LastSeen { get; set; }
        public string UserCode { get; set; }

        public string Status { get; set; }   

        public DateTime CreatedAt { get; set; }
        public bool IsBlocked { get; set; }
    }
}