
namespace YourProject.Models
{
    public class Vendor
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public int VenderId { get; set; }

        public string VenderCode { get; set; }
        public string? CodeDescription { get; set; }

        public string? LotVender { get; set; }

        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsVerifird { get; set; }

        public bool IsAgrementSigned { get; set; }
        public bool IsSignupCompleted { get; set; }

        public string Email { get; set; }

        public int UserId { get; set; }

        public bool IsDeleted { get; set; }
        public int? TotalCount { get; internal set; }

        internal static bool Any()
        {
            throw new NotImplementedException();
        }

        internal static object First()
        {
            throw new NotImplementedException();
        }
    }

}