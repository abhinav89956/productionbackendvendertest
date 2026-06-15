namespace VenderTest.DTOs
{
    public class BlockedUserDto
    {
        public int BlockedUserId { get; set; } 
        public int UserId { get; set; }        
        //public int BlockedByUserId { get; set; }
        public DateTime BlockedAt { get; set; } 
    }
}
