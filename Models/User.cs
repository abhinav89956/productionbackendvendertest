namespace VenderTest.Models
{
   
        public class User
        {
            public int UserId { get; set; }

            public string Email { get; set; }
            public string password { get; set; }
            public string VenderCode { get; set; }

            public string? UserCode { get; set; }

            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
