using VenderTest.Models;
using YourProject.Models;

namespace VenderTest.DTOs
{
    public class VenderAsignDto
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public string VenderCode { get; set; }
        public string ItemCode { get; set; }

    }
}
 